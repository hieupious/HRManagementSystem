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
    [Authorize(Roles = "NormalUser,Manager,Administrator,HRGroup")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IDailyWorkingProcessService dailyWorkingProcess;
        private readonly IWorkingHoursValidator workingHoursValidator;
        private DbSet<WorkingPoliciesGroup> WorkingPoliciesGroup;

        public UsersController(ApplicationDbContext dbContext, IDailyWorkingProcessService dailyWorkingProcess, IWorkingHoursValidator workingHoursValidator)
        {
            this.dbContext = dbContext;
            this.dailyWorkingProcess = dailyWorkingProcess;
            WorkingPoliciesGroup = dbContext.GetWorkingPoliciesGroups();
            this.workingHoursValidator = workingHoursValidator;
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
            return "Not Found";
        }

        [HttpGet("{empId}/{Report}/{month?}")]
        public string Report(int empId, DateTime? month)
        {
            if (User.IsInRole("NormalUser"))
            {
                empId = int.Parse(User.FindFirstValue(ClaimTypes.Sid));
            }
            if (!month.HasValue)
                month = DateTime.Now;
            var user = dbContext.UserInfoes.Where(u => int.Parse(u.EmployeeId) == empId).First();
            if (user == null)
                return null;
            if (user.WorkingPoliciesGroupId.HasValue)
                user.WorkingPoliciesGroup = WorkingPoliciesGroup.SingleOrDefault(w => w.Id == user.WorkingPoliciesGroupId.Value);
            var records = new List<DailyWorkingRecord>();
            foreach (var day in WorkingProcessService.AllDatesInMonth(month.Value.Year, month.Value.Month))
            {
                if (day <= DateTime.Now.Date && WorkingProcessService.IsWorkingDay(day))
                {
                    var record = dailyWorkingProcess.HandleWorkingReport(user.Id, day);
                    if(user.WorkingPoliciesGroupId.HasValue)
                    {                        
                        record.MinuteLate = workingHoursValidator.ValidateDailyRecord(record, user.WorkingPoliciesGroup, day);
                    }
                    records.Add(record);
                }
            }
            return JsonConvert.SerializeObject(records);
        }


        [HttpGet("ManagerList")]
        public string ManagerList()
        {
            var managers = dbContext.UserInfoes.Where(u => u.Role == Role.Manager);
            return JsonConvert.SerializeObject(managers);
        }

        [HttpGet("GetMonthlyWorkingReport")]
        public string GetMonthlyWorkingReport(int year, int month)
        {
            var monthRecords = dbContext.MonthlyRecords.Where(m => m.Month == month && m.Year == year);

            return JsonConvert.SerializeObject(monthRecords);
        }

        [HttpGet("ProcessDailyWorkingReport")]
        public string ProcessDailyWorkingReport()
        {
            int year = 2015;
            int month = 8;
            var users = dbContext.UserInfoes.ToList();
            foreach (var user in users)
            {
                var days = WorkingProcessService.AllDatesInMonth(year, month);
                foreach (var day in days)
                {
                    var records = dbContext.CheckInOutRecords.Where(u => u.UserId == user.Id && u.CheckTime.Date == day).ToList();
                    if (records != null && records.Count > 0)
                    {
                        var dailyRecord = dailyWorkingProcess.ProcessDailyWorking(user, records, day);
                        if (dailyRecord != null)
                            dbContext.DailyWorkingRecords.Add(dailyRecord);
                    }
                }
                dbContext.SaveChanges();
            }


            return "";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]DailyWorkingRecord value)
        {
            var record = dbContext.DailyWorkingRecords.FirstOrDefault(d => d.Id == value.Id);
            if (record != null)
            {
                record.GetApprovedReason = value.GetApprovedReason;
                record.ApproverId = value.ApproverId;
                dbContext.SaveChanges();
            }
            return new NoContentResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("GetPendingApproval")]
        public string GetPendingApproval()
        {
            var pendingRecords = dbContext.DailyWorkingRecords.Include(d => d.UserInfo).Include(d => d.Approver).Where(d => d.GetApprovedReason != null);
            return JsonConvert.SerializeObject(pendingRecords);
        }

        [HttpPut("Approval")]
        public IActionResult Approval([FromBody] DailyWorkingRecord value)
        {
            var record = dbContext.DailyWorkingRecords.FirstOrDefault(d => d.Id == value.Id);
            if (record != null)
            {
                record.Approved = value.Approved;
                record.ApproverComment = value.ApproverComment;
                dbContext.SaveChanges();
            }
            return new NoContentResult();
        }
    }
}
