using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public abstract class WorkingHoursRuleBase
    {
        public ICollection<UserGroup> UserGroups { get; set; }
    }

    public class BaseTimeWorkingHoursRule : WorkingHoursRuleBase
    {
        public TimeSpan WorkingTimeStart { get; set; }
        public TimeSpan WorkingTimeEnd { get; set; }
        public TimeSpan BreaktimeStart { get; set; }
        public TimeSpan BreaktimeEnd { get; set; }

        public DateTime WorkingTimeStartOnDate(DateTime date)
        {
            return TimeOnDate(date, WorkingTimeStart);
        }

        public DateTime WorkingTimeEndOnDate(DateTime date)
        {
            return TimeOnDate(date, WorkingTimeEnd);
        }

        public DateTime BreaktimeStartOnDate(DateTime date)
        {
            return TimeOnDate(date, BreaktimeStart);
        }

        public DateTime BreaktimeEndOnDate(DateTime date)
        {
            return TimeOnDate(date, BreaktimeEnd);
        }

        protected DateTime TimeOnDate(DateTime date, TimeSpan time)
        {
            return date.Date.Add(time);
        }
    }

    public class ToleranceWorkingHoursRule : WorkingHoursRuleBase
    {
        public TimeSpan Tolerance { get; set; }
    }

    public class EarlyToleranceWorkingHoursRule : ToleranceWorkingHoursRule
    {
    }

    public class LateToleranceWorkingHoursRule : ToleranceWorkingHoursRule
    {
    }
}
