using System.Collections.Generic;
using HRMS.Web.Models;
using System;

namespace HRMS.Web.Services
{
    public interface IImportDataService
    {
        IEnumerable<UserInfo> GetUserFromAccessDB();
        IEnumerable<Department> GetDepartmentFromAccessDB();
        IEnumerable<CheckInOutRecord> GetAllCheckInOutFromAccessDB();
        IEnumerable<CheckInOutRecord> GetDailyCheckInOutFromAccessDB();
        IEnumerable<CheckInOutRecord> GetWithDayCheckInOutFromAccessDB(DateTime fromDay, DateTime? toDay);
        bool CopyFileFromExternal();
    }
}