using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserInfo> Users { get; set; }
        public ICollection<WorkingHoursRuleBase> WorkingHoursRules { get; set; }
    }
}
