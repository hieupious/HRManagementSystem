﻿using System;
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
        private readonly ApplicationDbContext _dbContext;
        private readonly IDailyWorkingProcessService _dailyWorkingProcess;
        private readonly IMonthlyWorkingProcess _monthlyWorkingProcess;

        private static int[] activeDepts = { 2, 3, 6, 7, 8, 9, 10 };

        public UsersController(ApplicationDbContext dbContext, IDailyWorkingProcessService dailyWorkingProcess, IMonthlyWorkingProcess monthlyWorkingProcess)
        {
            _dbContext = dbContext;
            _dailyWorkingProcess = dailyWorkingProcess;
            _monthlyWorkingProcess = monthlyWorkingProcess;
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {
            
            var users = _dbContext.UserInfoes.Include(u => u.Department).Where(u => activeDepts.Contains(u.DepartmentId)).ToList();
            
            return JsonConvert.SerializeObject(users);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            
            return "value";
        }

        [HttpGet("GetMonthlyWorkingReport")]
        public string GetMonthlyWorkingReport(int year, int month)
        {
            var monthRecords = new List<MonthlyRecord>();
            var users = _dbContext.UserInfoes.Include(u => u.Department).Where(u => activeDepts.Contains(u.DepartmentId)).ToList();
            foreach(var user in users)
            {
                var days = WorkingProcessService.AllDatesInMonth(year, month);
                var dailyRecords = new List<DailyWorkingRecord>();
                foreach (var day in days)
                {
                    dailyRecords = _dbContext.DailyWorkingRecords.Where(d => d.UserId == user.Id && d.WorkingDay.Date == day).ToList();
                }
                
                var monthRecord = _monthlyWorkingProcess.GetMonthlyRecord(year, month, user, dailyRecords);
                if (monthRecord != null)
                    monthRecords.Add(monthRecord);
            }
            return JsonConvert.SerializeObject(monthRecords);
        }

        [HttpGet("ProcessDailyWorkingReport")]
        public string ProcessDailyWorkingReport()
        {
            int year = 2015;
            int month = 8;
            var users = _dbContext.UserInfoes.ToList();
            foreach(var user in users)
            {
                var days = WorkingProcessService.AllDatesInMonth(year, month);
                foreach(var day in days)
                {
                    var records = _dbContext.CheckInOutRecords.Where(u => u.UserId == user.Id && u.CheckTime.Date == day).ToList();
                    if(records != null && records.Count > 0)
                    {
                        var dailyRecord = _dailyWorkingProcess.ProcessDailyWorking(user, records, day);
                        if (dailyRecord != null)
                            _dbContext.DailyWorkingRecords.Add(dailyRecord);
                    }
                    
                }
                
            }
            _dbContext.SaveChanges();
            
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
