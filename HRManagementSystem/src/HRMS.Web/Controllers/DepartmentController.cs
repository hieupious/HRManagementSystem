using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Services;
using HRMS.Web.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Web.Controllers
{
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IDailyWorkingProcessService _dailyWorkingProcess;

        public DepartmentController(ApplicationDbContext dbContext, IDailyWorkingProcessService dailyWorkingProcess)
        {
            _dbContext = dbContext;
            _dailyWorkingProcess = dailyWorkingProcess;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Department> Get()
        {
            var listDepartment = _dbContext.Deparments.ToList();
            return listDepartment;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Department Get(int id)
        {
            var department = _dbContext.Deparments.Where(d => d.Id == id).First();
            return department;
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
