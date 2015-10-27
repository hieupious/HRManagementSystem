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
        IEnumerable<CheckInOutRecord> GetCheckInOutRecordWithDayFromAccessDB(DateTime fromDay, DateTime? toDay);
        int ImportCheckInOutFromAccessDBDaily();
        int ImportCheckInOutRecordWithDay(DateTime fromDay, DateTime? toDay = null, ApplicationDbContext exDbContext = null);
        bool CopyFileFromExternal(ref DateTime lastWriteTime);
    }
}