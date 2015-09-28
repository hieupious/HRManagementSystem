using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;

namespace HRMS.Web.Services
{
    public class WorkingProcessService : IDailyWorkingProcessService
    {
        public DailyWorkingReport ProcessDailyWorking(UserInfo user, List<CheckInOutRecord> checkInOutTime)
        {
            throw new NotImplementedException();
        }
    }
}
