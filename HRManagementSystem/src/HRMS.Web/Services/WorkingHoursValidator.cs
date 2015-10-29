using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;

namespace HRMS.Web.Services
{
    public class WorkingHoursValidator : IWorkingHoursValidator
    {
        public int Validate(UserInfo user, DateTime date)
        {
            // TODO:: Validate start > end, in > out...
            var record = user.DailyRecords.SingleOrDefault(m => m.WorkingDay == date);
            return ValidateRule(record, user.WorkingPoliciesGroup, date);
        }

        public int ValidateRule(DailyWorkingRecord record, WorkingPoliciesGroup policy, DateTime date)
        {
            var baseTimeRule = policy.WorkingHoursRules.SingleOrDefault(m => m is BaseTimeWorkingHoursRule) as BaseTimeWorkingHoursRule;
            var earlyToleranceRule = policy.WorkingHoursRules.SingleOrDefault(m => m is EarlyToleranceWorkingHoursRule) as EarlyToleranceWorkingHoursRule;
            var lateToleranceRule = policy.WorkingHoursRules.SingleOrDefault(m => m is LateToleranceWorkingHoursRule) as LateToleranceWorkingHoursRule;
            if (record == null || baseTimeRule == null)
            {
                throw new InvalidOperationException();
            }

            var breaktimeAmount = baseTimeRule.BreaktimeEnd - baseTimeRule.BreaktimeStart;
            var requiredTimeAmount = baseTimeRule.WorkingTimeEnd - baseTimeRule.WorkingTimeStart - breaktimeAmount;

            if (!record.CheckIn.HasValue || !record.CheckOut.HasValue)
            {
                return (int)requiredTimeAmount.TotalMinutes;
            }

            var earliestCheckin = baseTimeRule.WorkingTimeStartOnDate(date);
            if (earlyToleranceRule != null)
            {
                earliestCheckin = earliestCheckin.Add(-earlyToleranceRule.Tolerance);
            }

            var latestCheckout = baseTimeRule.WorkingTimeEndOnDate(date);
            if (lateToleranceRule != null)
            {
                latestCheckout = latestCheckout.Add(lateToleranceRule.Tolerance);
            }

            var checkIn = record.CheckIn.Value;
            // Exclude out of range time
            if (checkIn < earliestCheckin)
            {
                checkIn = earliestCheckin;
            }
            // Exclude breaktime
            if (checkIn >= baseTimeRule.BreaktimeStartOnDate(date) && checkIn <= baseTimeRule.BreaktimeEndOnDate(date))
            {
                checkIn = baseTimeRule.BreaktimeEndOnDate(date);
            }

            var checkOut = record.CheckOut.Value;
            // Exclude out of range time
            if (checkOut > latestCheckout)
            {
                checkOut = latestCheckout;
            }
            // Exclude breaktime
            if (checkOut >= baseTimeRule.BreaktimeStartOnDate(date) && checkOut <= baseTimeRule.BreaktimeEndOnDate(date))
            {
                checkOut = baseTimeRule.BreaktimeStartOnDate(date);
                // When both checkIn and checkOut in breaktime
                if (checkOut < checkIn)
                {
                    checkOut = checkIn;
                }
            }

            // set checkOut cannot be earlier than checkIn
            if (checkOut < checkIn)
                checkOut = checkIn;

            var workedTime = checkOut - checkIn;
            
            // If attendance covers breaktime, increase required time amount by breaktime amount
            if (checkIn < baseTimeRule.BreaktimeStartOnDate(date) && checkOut > baseTimeRule.BreaktimeEndOnDate(date))
            {
                workedTime -= breaktimeAmount;
            }

            var lackingTime = requiredTimeAmount - workedTime;
            return Math.Max((int)lackingTime.TotalMinutes, 0);
        }
        public int ValidateDailyRecord(DailyWorkingRecord dailyWorkingRecord, WorkingPoliciesGroup workingPolicyGroup, DateTime day)
        {
            return ValidateRule(dailyWorkingRecord, workingPolicyGroup, day);
        }
    }
}
