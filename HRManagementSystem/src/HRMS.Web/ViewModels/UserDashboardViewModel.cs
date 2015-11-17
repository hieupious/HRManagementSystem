using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.ViewModels
{
    public class UserInfoViewModel
    {
        public string Name { get; set; }

        public string EmployeeId { get; set; }

        public string Department { get; set; }

        public string Office { get; set; }

        public string WorkingHourRuleApplied { get; set; }

        public string CurrentDayCheckinTime { get; set; }

        public string TotalLackingTimeInMonth { get; set; }
    }
}
