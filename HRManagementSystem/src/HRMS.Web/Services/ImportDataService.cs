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
        private const string queryUserWithId = "SELECT [USERID], [Badgenumber], [Name], [DEFAULTDEPTID] from USERINFO WHERE ([USERID] = {0})";
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
            dbContext = Db.Open(dbPath);
        }

        public void ImportAllCheckInOutFromAccessDB()
        {
            var allCheckInOutRecords = GetAllCheckInOutFromAccessDB();
            efDbContext.CheckInOutRecords.AddRange(allCheckInOutRecords);
            efDbContext.SaveChanges();
        }

        public IEnumerable<CheckInOutRecord> GetAllCheckInOutFromAccessDB()
        {
            var checkInOutInfo = dbContext.ExecuteMany(queryAllCheckInOutRecord);
            return Mapper.MapMany<CheckInOutRecord, CheckInOutMapping>(checkInOutInfo);
        }

        public int ImportDailyCheckInOutFromAccessDB()
        {
            // only get records have userId in the system
            var dailyCheckInOutRecords = GetDailyCheckInOutFromAccessDB().Where(r => efDbContext.UserInfoes.Any(u => u.ExternalId == r.UserId));
            // filter duplicated records
            var filteredRecords = dailyCheckInOutRecords.Where(r => !efDbContext.CheckInOutRecords.Any(c => c.CheckTime == r.CheckTime && c.UserId == r.UserId));
            efDbContext.CheckInOutRecords.AddRange(filteredRecords);
            return efDbContext.SaveChanges();
        }

        public IEnumerable<CheckInOutRecord> GetDailyCheckInOutFromAccessDB()
        {
            return GetCheckInOutRecordWithDayFromAccessDB(DateTime.Now);
        }
        public IEnumerable<CheckInOutRecord> GetCheckInOutRecordWithDayFromAccessDB(DateTime fromDay, DateTime? toDay = null)
        {
            if (!toDay.HasValue)
                toDay = fromDay.AddDays(1);
            var query = string.Format(queryCheckInOutRecordWithDay, fromDay.Date.ToShortDateString(), toDay.Value.Date.ToShortDateString());
            var checkInOutInfo = dbContext.ExecuteMany(query);
            return Mapper.MapMany<CheckInOutRecord, CheckInOutMapping>(checkInOutInfo);
        }

        public int ImportCheckInOutRecordWithDay(DateTime fromDay, DateTime? toDay = null)
        {
            // only get records have userId in the system
            var records = GetCheckInOutRecordWithDayFromAccessDB(fromDay, toDay).Where(r => efDbContext.UserInfoes.Any(u => u.ExternalId == r.UserId));
            // filter duplicated records
            var filteredRecords = records.Where(r => !efDbContext.CheckInOutRecords.Any(c => c.CheckTime == r.CheckTime && c.UserId == r.UserId));
            efDbContext.CheckInOutRecords.AddRange(filteredRecords);
            return efDbContext.SaveChanges();
        }

        public IEnumerable<Department> GetDepartmentFromAccessDB()
        {
            var deptInfo = dbContext.ExecuteMany(queryDepartment);
            return Mapper.MapMany<Department, DepartmentMapping>(deptInfo);
        }

        public IEnumerable<UserInfo> GetUserFromAccessDB()
        {
            var userInfo = dbContext.ExecuteMany(queryUser);
            return Mapper.MapMany<UserInfo, UserMapping>(userInfo);
        }

        public UserInfo GetUserFromAccessDBWithId(int id)
        {
            var query = string.Format(queryUserWithId, id);
            var userInfo = dbContext.ExecuteSingle(query);
            return Mapper.Map<UserInfo, UserMapping>(userInfo);
        }

        public bool ImportUserInfoWithId(int id)
        {
            var userInfo = GetUserFromAccessDBWithId(id);
            if (userInfo != null)
            {
                efDbContext.UserInfoes.Add(userInfo);
                return efDbContext.SaveChanges() > 0;
            }
            return false;
        }


        public bool CopyFileFromExternal(ref DateTime lastWriteTime)
        {
            try
            {
                string fullFilePath = importConfiguration.FileToCopyPath;
                File.Copy(fullFilePath, dbPath, true);
                lastWriteTime = File.GetLastWriteTime(dbPath);
                return true;
            }
            catch
            {
                return false;
            }
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