using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using Newtonsoft.Json;
using Microsoft.Data.Entity;
using Hangfire;
using HRMS.Web.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Web.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IImportDataService importDataService;
        public ReportsController(ApplicationDbContext dbContext, IImportDataService importDataService)
        {
            this.dbContext = dbContext;
            this.importDataService = importDataService;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{month}")]
        public string Get(DateTime month)
        {
            var monthRecords = dbContext.MonthlyRecords.Include(m => m.UserInfo).ThenInclude(u => u.Department).Where(m => m.Month == month.Month && m.Year == month.Year).Include(m => m.DailyRecords);
            foreach(var record in monthRecords)
            {
                record.UserInfo = dbContext.UserInfoes.Include(u => u.Department).FirstOrDefault(u => u.Id == record.UserId);
            }
            return JsonConvert.SerializeObject(monthRecords);
        }

        [HttpGet("RunDailyImport")]
        public string RunDailyImport()
        {
            
            //RecurringJob.AddOrUpdate(() => importDataService.ImportDailyCheckInOutFromAccessDB(), Cron.Daily);

            return "Success";
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
