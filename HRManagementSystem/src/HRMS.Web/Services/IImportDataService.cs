using System.Collections.Generic;
using HRMS.Web.Models;
using System;

namespace HRMS.Web.Services
{
    public interface IImportDataService
    {
        IEnumerable<UserInfo> ImportUserFromAccessDB();
        IEnumerable<Department> ImportDepartmentFromAccessDB();
        IEnumerable<CheckInOutRecord> ImportAllCheckInOutFromAccessDB();
        IEnumerable<CheckInOutRecord> ImportDailyCheckInOutFromAccessDB();
        IEnumerable<CheckInOutRecord> ImportWithDayCheckInOutFromAccessDB(DateTime fromDay, DateTime? toDay);
        bool CopyFileFromExternal(ref DateTime lastWriteTime);
    }
}