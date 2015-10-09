using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;
using ekm.oledb.data;
using System.IO;
using HRMS.Web.Configuration;
using Microsoft.Framework.OptionsModel;

namespace HRMS.Web.Services
{
    public class ImportDataService : IImportDataService
    {
        private string dbPath;
        private DatabaseContext dbContext;
        private ImportConfiguration importConfiguration;
        private ApplicationDbContext efDbContext;
        // Query string 
        private const string queryUser = "SELECT [USERID], [Badgenumber], [Name], [DEFAULTDEPTID] from USERINFO";
        private const string queryDepartment = "SELECT [DEPTID], [DEPTNAME] FROM DEPARTMENTS";
        private const string queryAllCheckInOutRecord = "SELECT [USERID], [CHECKTIME] FROM CHECKINOUT";
        private const string queryCheckInOutRecordWithDay = "SELECT [USERID], [CHECKTIME] FROM CHECKINOUT WHERE (CHECKTIME >= #{0}#) AND (CHECKTIME <= #{1}#)";

        public ImportDataService()
        {

        }

        public ImportDataService(IOptions<ImportConfiguration> options, ApplicationDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
            importConfiguration = options.Options;
            dbPath = importConfiguration.ApplicationBasePath + "\\" + importConfiguration.ImportedDBPath;
            dbContext = Db.Open(this.dbPath);
        }

        public IEnumerable<CheckInOutRecord> ImportAllCheckInOutFromAccessDB()
        {
            var checkInOutInfo = dbContext.ExecuteMany(queryAllCheckInOutRecord);
            return Mapper.MapMany<CheckInOutRecord, CheckInOutMapping>(checkInOutInfo);
        }

        public IEnumerable<CheckInOutRecord> ImportDailyCheckInOutFromAccessDB()
        {
            return ImportWithDayCheckInOutFromAccessDB(DateTime.Now, null);
        }
        public IEnumerable<CheckInOutRecord> ImportWithDayCheckInOutFromAccessDB(DateTime fromDay, DateTime? toDay)
        {
            if (!toDay.HasValue)
                toDay = fromDay.AddDays(1);
            var query = string.Format(queryCheckInOutRecordWithDay, fromDay.Date.ToShortDateString(), toDay.Value.Date.ToShortDateString());
            var checkInOutInfo = dbContext.ExecuteMany(query);
            return Mapper.MapMany<CheckInOutRecord, CheckInOutMapping>(checkInOutInfo);
        }

        public IEnumerable<Department> ImportDepartmentFromAccessDB()
        {
            var deptInfo = dbContext.ExecuteMany(queryDepartment);
            return Mapper.MapMany<Department, DepartmentMapping>(deptInfo);
        }

        public IEnumerable<UserInfo> ImportUserFromAccessDB()
        {
            var userInfo = dbContext.ExecuteMany(queryUser);
            return Mapper.MapMany<UserInfo, UserMapping>(userInfo);
        }

        public bool CopyFileFromExternal()
        {
            try
            {
                string fullFilePath = importConfiguration.FileToCopyPath;
                
                File.Copy(fullFilePath, dbPath, true);
                //lastWriteTime = File.GetLastWriteTime(dbPath);

            } catch (Exception e)
            {
                
                return false;
            }

            return false;
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