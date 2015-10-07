using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;

namespace HRMS.Web.Services
{
    public interface IDailyWorkingProcessService
    {
        DailyWorkingRecord ProcessDailyWorking(UserInfo user, IEnumerable<CheckInOutRecord> checkInOutTime, DateTime day);
        DailyWorkingRecord GetDailyWorkingReport(int userId, DateTime day);
    }
}
