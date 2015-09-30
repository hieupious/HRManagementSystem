using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        //public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
