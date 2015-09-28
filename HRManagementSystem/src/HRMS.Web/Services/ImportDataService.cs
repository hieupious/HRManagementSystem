using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;
using ekm.oledb.data;

namespace HRMS.Web.Services
{
    public class ImportDataService : IImportDataService
    {
        private string _dbPath;
        private DatabaseContext _dbContext;
        private const string queryUser = "SELECT * from USERINFO";
        private const string queryDepartment = "SELECT * FROM DEPARTMENTS";
        private const string queryAllCheckInOutRecord = "SELECT * FROM CHECKINOUT";
        private const string queryDailyCheckInOutRecord = "SELECT * FROM CHECKINOUT WHERE CHECKTIME >= #{0}#";

        public ImportDataService(string dbPath)
        {
            _dbPath = dbPath;
            _dbContext = Db.Open(_dbPath);
        }

        public List<CheckInOutRecord> ImportAllCheckInOutFromAccessDB()
        {
            var checkInOutInfo = _dbContext.ExecuteMany(queryAllCheckInOutRecord);
            return Mapper.MapMany<CheckInOutRecord, CheckInOutMapping>(checkInOutInfo);
        }

        public List<CheckInOutRecord> ImportDailyCheckInOutFromAccessDB()
        {
            throw new NotImplementedException();
        }

        public List<Department> ImportDepartmentFromAccessDB()
        {
            var deptInfo = _dbContext.ExecuteMany(queryDepartment);
            return Mapper.MapMany<Department, DepartmentMapping>(deptInfo);
        }

        public List<UserInfo> ImportUserFromAccessDB()
        {
            var userInfo = _dbContext.ExecuteMany(queryUser);
            return Mapper.MapMany<UserInfo, UserMapping>(userInfo);
        }


    }

    public class UserMapping : ObjectMapping
    {
        public UserMapping()
        {
            Map("USERID", "Id");
            Map("Badgenumber", "EmployeeId");
            Map("Name", "Name");
            Map("DEFAULTDEPTID", "DepartmentId");
        }
    }

    public class DepartmentMapping : ObjectMapping
    {
        public DepartmentMapping()
        {
            Map("DEPTID", "Id");
            Map("DEPTNAME", "Name");
        }
    }
    public class CheckInOutMapping : ObjectMapping
    {
        public CheckInOutMapping()
        {
            Map("USERID", "UserId");
            Map("CHECKTIME", "CheckTime");
        }
    }
}