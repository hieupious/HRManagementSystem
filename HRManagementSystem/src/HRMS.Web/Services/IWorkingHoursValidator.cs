using HRMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Services
{
    public interface IWorkingHoursValidator
    {
        int Validate(UserInfo user, DateTime date);
        int ValidateDailyRecord(DailyWorkingRecord dailyWorkingRecord, WorkingPoliciesGroup workingPolicyGroup, DateTime day);
    }
}
