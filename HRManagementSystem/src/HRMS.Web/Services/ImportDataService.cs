﻿using System;
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
        private const string queryUser = "SELECT [USERID], [Badgenumber], [Name], [DEFAULTDEPTID] from USERINFO";
        private const string queryUserWithId = "SELECT [USERID], [Badgenumber], [Name], [DEFAULTDEPTID] from USERINFO WHERE ([USERID] = {0})";
        private const string queryDepartment = "SELECT [DEPTID], [DEPTNAME] FROM DEPARTMENTS";
        private const string queryAllCheckInOutRecord = "SELECT [USERID], [CHECKTIME] FROM CHECKINOUT";
        private const string queryCheckInOutRecordWithDay = "SELECT [USERID], [CHECKTIME] FROM CHECKINOUT WHERE (CHECKTIME >= #{0}#) AND (CHECKTIME < #{1}#)";

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

        public void ImportAllCheckInOutFromAccessDB()
        {
            var allCheckInOutRecords = GetAllCheckInOutFromAccessDB();
            dbContext.CheckInOutRecords.AddRange(allCheckInOutRecords);
            dbContext.SaveChanges();
        }

        public IEnumerable<CheckInOutRecord> GetAllCheckInOutFromAccessDB()
        {
            var checkInOutInfo = accessDbContext.ExecuteMany(queryAllCheckInOutRecord);
            return Mapper.MapMany<CheckInOutRecord, CheckInOutMapping>(checkInOutInfo);
        }

        public int ImportCheckInOutFromAccessDBDaily()
        {
            //// only get records have userId in the system
            //var dailyCheckInOutRecords = GetDailyCheckInOutFromAccessDB().Where(r => efDbContext.UserInfoes.Any(u => u.ExternalId == r.UserId));
            //// filter duplicated records
            //var filteredRecords = dailyCheckInOutRecords.Where(r => !efDbContext.CheckInOutRecords.Any(c => c.CheckTime == r.CheckTime && c.UserId == r.UserId));
            //efDbContext.CheckInOutRecords.AddRange(filteredRecords);
            //return efDbContext.SaveChanges();
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
            var query = string.Format(queryCheckInOutRecordWithDay, fromDay.Date.ToShortDateString(), toDay.Value.Date.ToShortDateString());
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
            var deptInfo = accessDbContext.ExecuteMany(queryDepartment);
            return Mapper.MapMany<Department, DepartmentMapping>(deptInfo);
        }

        public IEnumerable<UserInfo> GetUserFromAccessDB()
        {
            var userInfo = accessDbContext.ExecuteMany(queryUser);
            return Mapper.MapMany<UserInfo, UserMapping>(userInfo);
        }

        public UserInfo GetUserFromAccessDBWithId(int id)
        {
            var query = string.Format(queryUserWithId, id);
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