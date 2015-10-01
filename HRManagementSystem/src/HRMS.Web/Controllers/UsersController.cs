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
        private readonly ApplicationDbContext _dbContext;
        private readonly IDailyWorkingProcessService _dailyWorkingProcess;

        public UsersController(ApplicationDbContext dbContext, IDailyWorkingProcessService dailyWorkingProcess)
        {
            _dbContext = dbContext;
            _dailyWorkingProcess = dailyWorkingProcess;
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {
            int[] activeDepts = { 2, 3, 6, 7, 8, 9, 10 };
            var users = _dbContext.UserInfoes.Include(u => u.Department).Where(u => activeDepts.Contains(u.DepartmentId)).ToList();
            
            return JsonConvert.SerializeObject(users);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            
            return "value";
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
