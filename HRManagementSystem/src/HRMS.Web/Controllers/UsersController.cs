using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using HRMS.Web.Services;
using Microsoft.Data.Entity;
using Newtonsoft.Json;


namespace HRMS.Web.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IDailyWorkingProcessService dailyWorkingProcess;
        private readonly IMonthlyWorkingProcess monthlyWorkingProcess;

        private static int[] activeDepts = { 2, 3, 6, 7, 8, 9, 10 };

        public UsersController(ApplicationDbContext dbContext, IDailyWorkingProcessService dailyWorkingProcess, IMonthlyWorkingProcess monthlyWorkingProcess)
        {
            this.dbContext = dbContext;
            this.dailyWorkingProcess = dailyWorkingProcess;
            this.monthlyWorkingProcess = monthlyWorkingProcess;
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {
            var users = dbContext.UserInfoes.Include(u => u.Department).Where(u => activeDepts.Contains(u.DepartmentId));
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
            if (!month.HasValue)
                month = DateTime.Now;
            var user = dbContext.UserInfoes.Where(u => int.Parse(u.EmployeeId) == empId).First();
            if (user == null)
                return null;
            //var records = _dbContext.DailyWorkingRecords.Where(d => d.UserId == user.Id && d.WorkingDay.Month == 8 && d.WorkingDay.Year == month.Value.Year).OrderBy(u => u.WorkingDay);
            var records = new List<DailyWorkingRecord>();
            foreach(var day in WorkingProcessService.AllDatesInMonth(month.Value.Year, month.Value.Month))
            {
                if (day < DateTime.Now.AddDays(-1))
                {
                    var record = dailyWorkingProcess.GetDailyWorkingReport(user.Id, day);
                    if (record != null)
                        records.Add(record);
                }
            }
            return JsonConvert.SerializeObject(records);
        }

        [HttpGet("GetMonthlyWorkingReport")]
        public string GetMonthlyWorkingReport(int year, int month)
        {
            //var monthRecords = new List<MonthlyRecord>();
            //var users = _dbContext.UserInfoes.Include(u => u.Department).Where(u => activeDepts.Contains(u.DepartmentId)).ToList();
            //foreach (var user in users)
            //{
            //    var days = WorkingProcessService.AllDatesInMonth(year, month);
            //    var dailyRecords = _dbContext.DailyWorkingRecords.Where(d => d.WorkingDay.Month == month && d.WorkingDay.Year == year && d.UserId == user.Id).ToList();

            //    var monthRecord = _monthlyWorkingProcess.GetMonthlyRecord(year, month, user, dailyRecords);
            //    if (monthRecord != null)
            //    {
            //        monthRecords.Add(monthRecord);
            //        _dbContext.MonthlyRecords.Add(monthRecord);
            //        _dbContext.SaveChanges();
            //    }

            //}
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
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
