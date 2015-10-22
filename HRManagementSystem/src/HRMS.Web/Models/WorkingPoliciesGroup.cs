using System.Collections.Generic;

namespace HRMS.Web.Models
{
    public class WorkingPoliciesGroup
    {
        public WorkingPoliciesGroup()
        {
            WorkingHoursRules = new List<WorkingHoursRuleBase>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserInfo> Users { get; set; }
        public ICollection<WorkingHoursRuleBase> WorkingHoursRules { get; set; }
    }
}
