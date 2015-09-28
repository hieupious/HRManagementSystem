using ekm.oledb.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace HRMS.Business
{
    public class ImportDataService
    {
        private const string dbPath = "Sontest.mdb";
        public static void ImportAll()
        {
            var dbContext = Db.Open(dbPath);
            var userInfo = dbContext.ExecuteMany("SELECT * from USERINFO");
            var users = Mapper.MapMany<UserInfo, UserMapping>(userInfo);
            //WriteLine("ID \t EmpId \t Name \t\t DeptId");
            //foreach (var user in users)
            //{
            //    WriteLine("{0} \t {1} \t {2} \t\t {3}", user.UserId, user.EmployeeId, user.Name, user.DeptId);
            //}

            var deptInfo = dbContext.ExecuteMany("SELECT * FROM DEPARTMENTS");
            var depts = Mapper.MapMany<Department, DepartmentMapping>(deptInfo);
            //WriteLine("*******************************");
            //foreach (var dept in depts)
            //{
            //    WriteLine("{0} \t {1}", dept.DeptId, dept.Name);
            //}
            string queryString = $"SELECT * FROM CHECKINOUT WHERE CHECKTIME >= #{DateTime.Now.AddDays(-5).ToShortDateString()}#";
            var checkInOutInfo = dbContext.ExecuteMany(queryString);
            dbContext.ExecuteMany("SELECT * FROM CHECKINOUT");
            var checkInOuts = Mapper.MapMany<CheckInOutRecord, CheckInOutMapping>(checkInOutInfo);
            //WriteLine("*******************************");
            //foreach (var checkInOut in checkInOuts)
            //{
            //    WriteLine("{0} \t {1}", checkInOut.UserId, checkInOut.CheckTime);
            //}
        }

    }

    public class UserMapping : ObjectMapping
    {
        public UserMapping()
        {
            Map("USERID", "UserId");
            Map("Badgenumber", "EmployeeId");
            Map("Name", "Name");
            Map("DEFAULTDEPTID", "DeptId");
        }
    }

    public class DepartmentMapping : ObjectMapping
    {
        public DepartmentMapping()
        {
            Map("DEPTID", "DeptId");
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
