using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using HRMS.Web.Services;
using Microsoft.Data.Entity;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNet.Authorization;

namespace HRMS.Web.Controllers
{

    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IDailyWorkingProcessService dailyWorkingProcess;
        private readonly IMonthlyWorkingProcess monthlyWorkingProcess;
        private readonly IWorkingHoursValidator workingHoursValidator;
        private DbSet<WorkingPoliciesGroup> _policies;
        private DbSet<WorkingPoliciesGroup> WorkingPoliciesGroup
        {
            get
            {
                if(_policies == null)
                {
                    _policies = dbContext.GetWorkingPoliciesGroups();
                }
                return _policies;
            }
        }

        public UsersController(ApplicationDbContext dbContext, IDailyWorkingProcessService dailyWorkingProcess, IWorkingHoursValidator workingHoursValidator,
            IMonthlyWorkingProcess monthlyWorkingProcess)
        {
            this.dbContext = dbContext;
            this.dailyWorkingProcess = dailyWorkingProcess;
            this.workingHoursValidator = workingHoursValidator;
            this.monthlyWorkingProcess = monthlyWorkingProcess;
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {
            var users = dbContext.UserInfoes.Include(u => u.Department);
            return JsonConvert.SerializeObject(users);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var user = dbContext.UserInfoes.Include(u => u.Department).FirstOrDefault(u => u.Id == id);
            if (user != null)
                return JsonConvert.SerializeObject(user);
            return null;
        }

        [Authorize(Roles = "NormalUser,Manager,Administrator,HRGroup")]
        [HttpGet("{empId}/{Report}/{month?}")]
        public string Report(int empId, DateTime? month)
        {
            if (User.IsInRole("NormalUser"))
            {
                empId = int.Parse(User.FindFirstValue(ClaimTypes.Sid));
            }
            if (!month.HasValue)
                month = DateTime.Now;
            var user = dbContext.UserInfoes.Where(u => int.Parse(u.EmployeeId) == empId).FirstOrDefault();
            if (user == null)
                return null;
            if (user.WorkingPoliciesGroupId.HasValue)
            {

                user.WorkingPoliciesGroup = WorkingPoliciesGroup.SingleOrDefault(w => w.Id == user.WorkingPoliciesGroupId.Value);
            }
            var records = new List<DailyWorkingRecord>();
            foreach (var day in WorkingProcessService.AllDatesInMonth(month.Value.Year, month.Value.Month))
            {
                var record = dbContext.DailyWorkingRecords.Include(d => d.Approver).Include(d => d.CheckInOutRecords).FirstOrDefault(d => d.WorkingDay.Date == day.Date && d.UserInfoId == user.Id);
                if (record != null)
                {
                    record.MinuteLate = workingHoursValidator.ValidateDailyRecord(record, user.WorkingPoliciesGroup, day);
                    records.Add(record);
                }
            }
            return JsonConvert.SerializeObject(records.OrderByDescending(r => r.WorkingDay));
        }

        [Authorize(Roles = "NormalUser,Manager,Administrator,HRGroup")]
        [HttpGet("ManagerList")]
        public string ManagerList()
        {
            var managers = dbContext.UserInfoes.Where(u => u.Role == Role.Manager || u.Role == Role.HRGroup);
            return JsonConvert.SerializeObject(managers);
        }

        [Authorize(Roles = "Administrator,HRGroup")]
        [HttpGet("GetMonthlyWorkingReport")]
        public string GetMonthlyWorkingReport(DateTime month)
        {
            var monthRecords = monthlyWorkingProcess.GetMonthlyRecords(2015, 10);
            return JsonConvert.SerializeObject(monthRecords);
        }


        // PUT api/values/5
        [Authorize(Roles = "NormalUser,Manager,Administrator,HRGroup")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]DailyWorkingRecord value)
        {
            var record = dbContext.DailyWorkingRecords.FirstOrDefault(d => d.Id == value.Id);
            if (record != null)
            {
                // if approve/reject, cannot update
                if (record.Approved.HasValue)
                    return new HttpUnauthorizedResult();
                // get current user is userid of dailyworking record
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId != record.UserInfoId)
                    return new HttpUnauthorizedResult();
                // check whether ApproverId in Manager List
                if (!dbContext.UserInfoes.Where(u => u.Role == Role.Manager || u.Role == Role.HRGroup).Any(u => u.Id == value.ApproverId))
                    return new HttpUnauthorizedResult();

                record.GetApprovedReason = value.GetApprovedReason;
                record.ApproverId = value.ApproverId;
                dbContext.SaveChanges();
                return new HttpOkResult();
            }
            return new HttpNotFoundResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Authorize(Roles = "Manager,Administrator,HRGroup")]
        [HttpGet("GetPendingApproval")]
        public string GetPendingApproval()
        {
            var pendingRecords = new List<DailyWorkingRecord>();
            if (User.IsInRole(Role.Manager.ToString()) || User.IsInRole(Role.HRGroup.ToString()))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                pendingRecords = dbContext.DailyWorkingRecords.Include(d => d.UserInfo).Include(d => d.Approver).Include(d => d.CheckInOutRecords).Where(d => d.ApproverId == int.Parse(userId)).ToList();
            }
            else
            {
                pendingRecords = dbContext.DailyWorkingRecords.Include(d => d.UserInfo).Include(d => d.Approver).Include(d => d.CheckInOutRecords).Where(d => d.GetApprovedReason != null).ToList();
            }
            foreach (var record in pendingRecords)
            {
                if(record.UserInfo.WorkingPoliciesGroupId.HasValue)
                    record.MinuteLate = workingHoursValidator.ValidateDailyRecord(record, WorkingPoliciesGroup.SingleOrDefault(w => w.Id == record.UserInfo.WorkingPoliciesGroupId.Value), record.WorkingDay);
            }
            return JsonConvert.SerializeObject(pendingRecords);
        }

        [Authorize(Roles = "Manager,Administrator,HRGroup")]
        [HttpPut("Approval")]
        public IActionResult Approval([FromBody] DailyWorkingRecord value)
        {
            var record = dbContext.DailyWorkingRecords.FirstOrDefault(d => d.Id == value.Id);
            if (record != null)
            {
                // check only current user is manager, hrgroup, administrator is updated approval.
                // check if their id is approverid, they will permit to update.
                if (!record.ApproverId.HasValue || record.ApproverId.Value != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                    return new HttpUnauthorizedResult();
                record.Approved = value.Approved;
                record.ApproverComment = value.ApproverComment;
                dbContext.SaveChanges();
                return new HttpOkResult();
            }
            return new HttpNotFoundResult();
        }
    }
}
