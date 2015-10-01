using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using HRMS.Web.Services;
using Microsoft.Data.Entity;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDailyWorkingProcessService _dailyWorkingProcess;

        public UserController(ApplicationDbContext dbContext, IDailyWorkingProcessService dailyWorkingProcess)
        {
            _dbContext = dbContext;
            _dailyWorkingProcess = dailyWorkingProcess;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<UserInfo> Get()
        {
            return _dbContext.UserInfoes.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public UserInfo Get(int id)
        {
            var user = _dbContext.UserInfoes.Include(u => u.Department).Where(u => u.Id == id).First();
            //user.Department.UserInfoes = null;
            
            return user;
        }

        [HttpGet("GetDailyWorkingReport")]
        public DailyWorkingReport GetDailyWorkingReport(int userId, DateTime day)
        {
            var user = _dbContext.UserInfoes.Where(u => u.Id == userId).First();
            //var day = new DateTime(2015, 8, 19);
            var records = _dbContext.CheckInOutRecords.Where(u => u.UserId == user.Id && u.CheckTime.Date == day).ToList();
            var result = _dailyWorkingProcess.ProcessDailyWorking(user, records, day);
            return result;
        }

        [HttpGet("GetMonthlyWorkingReport")]
        public IEnumerable<DailyWorkingReport> GetMonthlyWorkingReport(int userId, int month)
        {

            var user = _dbContext.UserInfoes.Where(u => u.Id == userId).First();
            //var day = new DateTime(2015, 8, 19);
            var days = WorkingProcessService.AllDatesInMonth(DateTime.Now.Year, month);
            foreach(var day in days)
            {
                var records = _dbContext.CheckInOutRecords.Where(u => u.UserId == user.Id && u.CheckTime.Date == day).ToList();
                var result = _dailyWorkingProcess.ProcessDailyWorking(user, records, day);
                yield return result;
            }
            
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
