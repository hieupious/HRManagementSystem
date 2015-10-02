using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;

namespace HRMS.Web.Services
{
    public interface IMonthlyWorkingProcess
    {
        MonthlyRecord GetMonthlyRecord(int year, int month, UserInfo user, IEnumerable<DailyWorkingRecord> dailyRecords);
    }
}
