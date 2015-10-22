using System;
using System.Collections.Generic;

namespace HRMS.Web.Models
{
    public abstract class WorkingHoursRuleBase
    {
        public int Id { get; set; }
        public ICollection<WorkingPoliciesGroup> WorkingPoliciesGroups { get; set; }
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

    public abstract class ToleranceWorkingHoursRuleBase : WorkingHoursRuleBase
    {
        public TimeSpan Tolerance { get; set; }
    }

    public class EarlyToleranceWorkingHoursRule : ToleranceWorkingHoursRuleBase
    {
    }

    public class LateToleranceWorkingHoursRule : ToleranceWorkingHoursRuleBase
    {
    }
}
