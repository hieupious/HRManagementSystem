using System.Collections.Generic;
using HRMS.Web.Models;

namespace HRMS.Web.Services
{
    public interface IImportDataService
    {
        List<UserInfo> ImportUserFromAccessDB();
        List<Department> ImportDepartmentFromAccessDB();
        List<CheckInOutRecord> ImportAllCheckInOutFromAccessDB();
        List<CheckInOutRecord> ImportDailyCheckInOutFromAccessDB();
    }
}