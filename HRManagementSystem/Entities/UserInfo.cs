using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public int DeptId { get; set; }
        public Department Deparment { get; set; }
    }
}
