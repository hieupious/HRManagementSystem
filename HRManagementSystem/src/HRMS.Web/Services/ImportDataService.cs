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
        private DatabaseContext accessDbContext;
        private ImportConfiguration importConfiguration;
        private ApplicationDbContext dbContext;
        // Query string 
        private const string QueryUser = "SELECT [USERID], [Badgenumber], [Name], [DEFAULTDEPTID] from USERINFO";
        private const string QueryUserWithId = "SELECT [USERID], [Badgenumber], [Name], [DEFAULTDEPTID] from USERINFO WHERE ([USERID] = {0})";
        private const string QueryDepartment = "SELECT [DEPTID], [DEPTNAME] FROM DEPARTMENTS";
        private const string QueryAllCheckInOutRecord = "SELECT [USERID], [CHECKTIME] FROM CHECKINOUT";
        private const string QueryCheckInOutRecordWithDay = "SELECT [USERID], [CHECKTIME] FROM CHECKINOUT WHERE (CHECKTIME >= #{0}#) AND (CHECKTIME < #{1}#)";
        private const string QueryGetUserCheckTime = "SELECT CHECKINOUT.CHECKTIME FROM(CHECKINOUT INNER JOIN USERINFO ON CHECKINOUT.USERID = USERINFO.USERID) WHERE(CHECKINOUT.CHECKTIME >= #{0}#) AND (CHECKINOUT.CHECKTIME < #{1}#) AND (USERINFO.Badgenumber = '{2}')";

        public ImportDataService()
        {
        }

        public ImportDataService(IOptions<ImportConfiguration> options, ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            importConfiguration = options.Options;
            dbPath = importConfiguration.ApplicationBasePath + "\\" + importConfiguration.ImportedDBPath;
            accessDbContext = Db.Open(dbPath);
        }

        public IEnumerable<DateTime> GetUserCheckTime(UserInfo user, DateTime day)
        {
            if (user == null || day == null)
                throw new InvalidOperationException();
            var query = string.Format(QueryGetUserCheckTime, day.Date, day.AddDays(1).Date, user.FingerPrintId);
            var userCheckTime = accessDbContext.ExecuteMany(query);
            var result = new List<DateTime>();
            foreach (var ct in userCheckTime)
            {
                foreach (var r in (ct as IDictionary<string, object>))
                {
                    result.Add((DateTime)r.Value);
                }
            }
            return result;
        }
        public IEnumerable<CheckInOutRecord> GetAllCheckInOutFromAccessDB()
        {
            var checkInOutInfo = accessDbContext.ExecuteMany(QueryAllCheckInOutRecord);
            return Mapper.MapMany<CheckInOutRecord, CheckInOutMapping>(checkInOutInfo);
        }

        public int ImportCheckInOutFromAccessDBDaily()
        {
            return ImportCheckInOutRecordWithDay(DateTime.Now);
        }

        public IEnumerable<CheckInOutRecord> GetDailyCheckInOutFromAccessDB()
        {
            return GetCheckInOutRecordWithDayFromAccessDB(DateTime.Now);
        }
        public IEnumerable<CheckInOutRecord> GetCheckInOutRecordWithDayFromAccessDB(DateTime fromDay, DateTime? toDay = null)
        {
            if (!toDay.HasValue)
                toDay = fromDay.AddDays(1);
            var query = string.Format(QueryCheckInOutRecordWithDay, fromDay.Date.ToShortDateString(), toDay.Value.Date.ToShortDateString());
            var checkInOutInfo = accessDbContext.ExecuteMany(query);
            return Mapper.MapMany<CheckInOutRecord, CheckInOutMapping>(checkInOutInfo);
        }

        public int ImportCheckInOutRecordWithDay(DateTime fromDay, DateTime? toDay = null)
        {
            // only get records have userId in the system
            var records = GetCheckInOutRecordWithDayFromAccessDB(fromDay, toDay).Where(r => dbContext.UserInfoes.Any(u => u.ExternalId == r.UserId && u.WorkingPoliciesGroupId.HasValue));
            // filter duplicated records
            var filteredRecords = records.Where(r => !dbContext.CheckInOutRecords.Any(c => c.CheckTime == r.CheckTime && c.UserId == r.UserId));
            dbContext.CheckInOutRecords.AddRange(filteredRecords);
            return dbContext.SaveChanges();
        }

        public IEnumerable<Department> GetDepartmentFromAccessDB()
        {
            var deptInfo = accessDbContext.ExecuteMany(QueryDepartment);
            return Mapper.MapMany<Department, DepartmentMapping>(deptInfo);
        }

        public IEnumerable<UserInfo> GetUserFromAccessDB()
        {
            var userInfo = accessDbContext.ExecuteMany(QueryUser);
            return Mapper.MapMany<UserInfo, UserMapping>(userInfo);
        }

        public UserInfo GetUserFromAccessDBWithId(int id)
        {
            var query = string.Format(QueryUserWithId, id);
            var userInfo = accessDbContext.ExecuteSingle(query);
            return Mapper.Map<UserInfo, UserMapping>(userInfo);
        }

        public bool ImportUserInfoWithId(int id)
        {
            var userInfo = GetUserFromAccessDBWithId(id);
            if (userInfo != null)
            {
                dbContext.UserInfoes.Add(userInfo);
                return dbContext.SaveChanges() > 0;
            }
            return false;
        }


        public bool CopyFileFromExternal(ref DateTime lastWriteTime)
        {

            string fullFilePath = importConfiguration.FileToCopyPath;
            File.Copy(fullFilePath, dbPath, true);
            File.SetAttributes(dbPath, FileAttributes.Normal);
            lastWriteTime = File.GetLastWriteTime(dbPath);
            return true;

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