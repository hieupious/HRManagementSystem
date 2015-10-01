using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HRMS.Web.Models
{
    public class UserInfo
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
