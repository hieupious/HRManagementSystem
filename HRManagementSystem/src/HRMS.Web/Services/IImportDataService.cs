using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
