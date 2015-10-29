using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;
using Microsoft.Data.Entity;

namespace HRMS.Web.Services
{
    public class WorkingProcessService : IDailyWorkingProcessService, IMonthlyWorkingProcess
    {
        private ApplicationDbContext dbContext;
        private IWorkingHoursValidator workingHourValidator;
        public WorkingProcessService(ApplicationDbContext dbContext, IWorkingHoursValidator workingHourValidator)
        {
            this.dbContext = dbContext;
            this.workingHourValidator = workingHourValidator;
        }

        public void ProcessDailyWorkingReport(DateTime day)
        {
            // get list of activated user >> 
            var listActivatedUser = dbContext.UserInfoes.Where(u => u.WorkingPoliciesGroupId.HasValue).Select(u => u);
            foreach (var user in listActivatedUser)
            {
                if (IsWorkingDay(day, user))
                {
                    Console.WriteLine("User: " + user.Name + " - Day: " + day);
                    var userRecordOfDay = dbContext.CheckInOutRecords.Where(c => c.CheckTime.Date == day.Date && c.UserId == user.ExternalId && c.DailyRecordId == null);
                    var dailyRecordOfDay = dbContext.DailyWorkingRecords.SingleOrDefault(d => d.WorkingDay.Date == day.Date && d.UserInfoId == user.Id);
                    if (dailyRecordOfDay != null)
                    {
                        if (userRecordOfDay == null)
                        {
                            dailyRecordOfDay.CheckInOutRecords = null;
                            dbContext.Entry(dailyRecordOfDay).State = EntityState.Modified;
                        }
                        else
                        {
                            foreach (var r in userRecordOfDay)
                            {
                                r.DailyRecordId = dailyRecordOfDay.Id;
                                dbContext.Entry(r).State = EntityState.Modified;
                            }
                        }
                    }
                    else
                    {
                        DailyWorkingRecord dailyRecord = new DailyWorkingRecord()
                        {
                            UserInfoId = user.Id,
                            CheckInOutRecords = userRecordOfDay != null ? userRecordOfDay.ToList() : null,
                            WorkingDay = day.Date
                        };
                        dbContext.DailyWorkingRecords.Add(dailyRecord);
                    }
                }
            }
            dbContext.SaveChanges();
        }

        public bool IsWorkingDay(DateTime day, UserInfo user = null)
        {
            if(user != null)
            {
                if (!user.StartWorkingDay.HasValue || day.Date < user.StartWorkingDay.Value.Date)
                    return false;
            }
            if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday
                && !PublicHolidays(day.Year).Exists(d => d.Date == day.Date))
                return true;
            return false;
        }

        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }

        public static IEnumerable<DateTime> DaysBetweenTwoDays(DateTime fromDay, DateTime toDay)
        {
            if (fromDay.Date > toDay.Date)
                throw new InvalidOperationException();
            for (var day = fromDay.Date; day <= toDay.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        public static List<DateTime> PublicHolidays(int year)
        {
            DateTime newYearDay = new DateTime(year, 1, 1);
            DateTime liberationDay = new DateTime(year, 4, 30);
            DateTime internationalWorkerDay = new DateTime(year, 5, 1);
            DateTime nationalDay = new DateTime(year, 9, 2);
            List<DateTime> holidays = new List<DateTime>()
            {
                newYearDay, liberationDay, internationalWorkerDay, nationalDay
            };

            //TODO: need to add holidays for each year - Tet, Hung King

            return holidays;
        }


        /// <summary>
        /// Get monthly report records 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ICollection<MonthlyRecord> GetMonthlyRecords(int year, int month)
        {
            var violatedRecords = dbContext.DailyWorkingRecords.Include(d => d.CheckInOutRecords).Include(u => u.UserInfo).Where(
                d => d.WorkingDay.Year == year && d.WorkingDay.Month == month && d.Approved != true && d.UserInfo.WorkingPoliciesGroupId.HasValue
                ).GroupBy(v => v.UserInfoId).ToList();
            var count = violatedRecords.Count();
            ICollection<MonthlyRecord> monthRecords = new List<MonthlyRecord>();
            var WorkingPolicyGroup = dbContext.GetWorkingPoliciesGroups();
            foreach (var grpRecords in violatedRecords)
            {
                foreach (var record in grpRecords)
                {
                    record.MinuteLate = workingHourValidator.ValidateDailyRecord(record, WorkingPolicyGroup.SingleOrDefault(w => w.Id == record.UserInfo.WorkingPoliciesGroupId.Value), record.WorkingDay);
                }
                if (grpRecords.Sum(s => s.MinuteLate) > 30)
                {
                    var monthRecord = new MonthlyRecord()
                    {
                        Month = month,
                        Year = year,
                        UserInfoId = grpRecords.Key,
                        DailyRecords = grpRecords.Where(d => d.MinuteLate > 0).OrderBy(d => d.WorkingDay).ToList(),
                        TotalLackTime = grpRecords.Sum(s => s.MinuteLate),
                        UserInfo = grpRecords.First().UserInfo
                    };
                    monthRecords.Add(monthRecord);
                }
            }
            return monthRecords;
        }
    }
}
