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
        public IEnumerable<CheckInOutRecord> GetCheckInOutRecordOfDay(UserInfo user, DateTime day)
        {
            return dbContext.CheckInOutRecords.Where(u => u.UserId == user.ExternalId && u.CheckTime.Date == day);
        }

        public DailyWorkingRecord HandleWorkingReport(int userId, DateTime day)
        {
            var user = dbContext.UserInfoes.Include(u => u.DailyRecords).ThenInclude(d => d.CheckInOutRecords).SingleOrDefault(u => u.Id == userId);
            var userRecordOfDay = dbContext.CheckInOutRecords.Where(c => c.CheckTime.Date == day.Date && c.UserId == user.ExternalId);
            if (user != null)
            {
                var dailyRecordOfDay = user.DailyRecords.SingleOrDefault(d => d.WorkingDay.Date == day.Date);
                if (dailyRecordOfDay != null)
                {
                    var newRecords = userRecordOfDay.Where(c => c.DailyRecordId == null);
                    if (newRecords != null && newRecords.Count() > 0)
                    {
                        foreach (var r in newRecords)
                            dailyRecordOfDay.CheckInOutRecords.Add(r);
                        dbContext.SaveChanges();
                    }
                    return dailyRecordOfDay;
                }
                else
                {
                    DailyWorkingRecord dailyRecord = new DailyWorkingRecord()
                    {
                        UserInfoId = user.Id,
                        CheckInOutRecords = userRecordOfDay != null ? userRecordOfDay.ToList() : null,
                        WorkingDay = day
                    };
                    dbContext.DailyWorkingRecords.Add(dailyRecord);
                    dbContext.SaveChanges();
                    return dailyRecord;
                }

            }

            return null;
        }

        public DailyWorkingRecord GetDailyWorkingReport(UserInfo user, DateTime day, DailyWorkingRecord existedRecord = null)
        {
            if (user != null)
            {
                var checkInOutRecords = GetCheckInOutRecordOfDay(user, day);
                return ProcessDailyWorking(user, checkInOutRecords, day, existedRecord);
            }
            return null;
        }
        public DailyWorkingRecord ProcessDailyWorking(UserInfo user, IEnumerable<CheckInOutRecord> checkInOutRecords, DateTime day, DailyWorkingRecord existedRecord = null)
        {
            if (IsWorkingDay(day))
            {

                DailyWorkingRecord workingReport = new DailyWorkingRecord()
                {
                    UserInfoId = user.Id,
                    WorkingDay = day
                };

                if (checkInOutRecords != null && checkInOutRecords.Count() > 0)
                {
                    // var recordsOfDay = 
                    var group = checkInOutRecords.GroupBy(c => c.CheckTime.Date);
                    var sortedList = checkInOutRecords.OrderBy(c => c.CheckTime);
                    //workingReport.CheckIn = sortedList.First().CheckTime;
                    //workingReport.CheckOut = sortedList.Last().CheckTime;
                    //workingReport.CheckInOutRecords = checkInOutRecords.
                }
                else
                {
                    //workingReport.CheckIn = null;
                    //workingReport.CheckOut = null;
                }
                if (existedRecord != null)
                {
                    //existedRecord.CheckIn = workingReport.CheckIn;
                    //existedRecord.CheckOut = workingReport.CheckOut;
                    return existedRecord;
                }
                return workingReport;
            }
            return null;
        }

        public static bool IsWorkingDay(DateTime day)
        {
            if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday && !PublicHolidays(day.Year).Exists(d => d == day))
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
            if (fromDay.Date < toDay.Date)
                throw new InvalidOperationException();
            for(var day = fromDay.Date; day <= toDay.Date; day.AddDays(1))
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

        public MonthlyRecord GetMonthlyRecord(int year, int month, UserInfo user, IEnumerable<DailyWorkingRecord> dailyRecords)
        {
            if (user != null && dailyRecords != null)
            {
                MonthlyRecord monthlyRecord = new MonthlyRecord()
                {
                    Month = month,
                    Year = year,
                    UserInfoId = user.Id
                };
                var reportRecords = new List<DailyWorkingRecord>();

                var lackTimeRecords = dailyRecords.Where(d => d.MinuteLate > 0).ToList();
                reportRecords.AddRange(lackTimeRecords);

                var totalLackTime = dailyRecords.Sum(d => d.MinuteLate);
                if (totalLackTime > WorkingRule.TotalLackTime)
                {
                    monthlyRecord.DailyRecords = reportRecords;
                    monthlyRecord.TotalLackTime = totalLackTime;
                    return monthlyRecord;
                }
            }

            return null;
        }
    }

    public class WorkingRule
    {
        public static TimeSpan MinCheckInTime = new TimeSpan(7, 0, 0);
        public static TimeSpan MaxCheckInTime = new TimeSpan(9, 0, 0);
        public static TimeSpan MinCheckOutTime = new TimeSpan(16, 0, 0);
        public static TimeSpan MaxCheckOutTime = new TimeSpan(18, 0, 0);
        public static TimeSpan FullWorkingHour = new TimeSpan(9, 0, 0);
        public static TimeSpan HalfWorkingHour = new TimeSpan(4, 0, 0);
        public static TimeSpan NoonHour = new TimeSpan(12, 0, 0);
        public static double TotalLackTime = 30d;
    }

    public class DevWorkingRule : WorkingRule
    {

    }

}
