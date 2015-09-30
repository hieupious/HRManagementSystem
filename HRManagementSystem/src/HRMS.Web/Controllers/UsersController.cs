using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Web.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<dynamic> Get()
        {
            return new dynamic[] {
                new {
                    Id = 223,
                    Name = "Tran Quoc Linh",
                    Department = new {
                        Id = 7,
                        Name = "2XX GP OFFICE"
                    }
                },
                new {
                    Id = 209,
                    Name = "Nguyen Luong Yen Vy",
                    Department = new {
                        Id = 7,
                        Name = "2XX GP OFFICE"
                    }
                }
            };
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
