using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;

namespace HRMS.Web.Services
{
    public class WorkingProcessService : IDailyWorkingProcessService, IMonthlyWorkingProcess
    {
        private ApplicationDbContext dbContext;
        public WorkingProcessService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<CheckInOutRecord> GetCheckInOutRecordPerDay(UserInfo user, DateTime day)
        {
            return dbContext.CheckInOutRecords.Where(u => u.UserId == user.Id && u.CheckTime.Date == day);
        }

        public DailyWorkingRecord GetDailyWorkingReport(int userId, DateTime day)
        {
            var user = dbContext.UserInfoes.First(u => u.Id == userId);
            if (user != null)
            {
                var checkInOutRecords = GetCheckInOutRecordPerDay(user, day);
                return ProcessDailyWorking(user, checkInOutRecords, day);
            }
            return null;
        }
        public DailyWorkingRecord ProcessDailyWorking(UserInfo user, IEnumerable<CheckInOutRecord> checkInOutTime, DateTime day)
        {
            if (IsWorkingDay(day))
            {
                DailyWorkingRecord workingReport = new DailyWorkingRecord()
                {
                    UserInfoId = user.Id,
                    WorkingDay = day
                };
                if (checkInOutTime != null && checkInOutTime.Count() > 0)
                {
                    if (checkInOutTime.Count() == 1)
                    {
                        workingReport.CheckIn = checkInOutTime.First().CheckTime;
                        workingReport.CheckOut = null;
                        workingReport.WorkingType = WorkingType.LackCheckOut;
                    }
                    else
                    {
                        var sortedList = checkInOutTime.OrderBy(c => c.CheckTime);
                        var firstCheckInTime = sortedList.First();
                        workingReport.CheckIn = firstCheckInTime.CheckTime;
                        workingReport.CheckOut = sortedList.Last().CheckTime;
                        var startTime = (workingReport.CheckIn.Value.TimeOfDay < WorkingRule.MinCheckInTime) ? WorkingRule.MinCheckInTime : workingReport.CheckIn.Value.TimeOfDay;

                        var finishTime = (workingReport.CheckOut.Value.TimeOfDay > WorkingRule.MaxCheckOutTime) ? WorkingRule.MaxCheckOutTime : workingReport.CheckOut.Value.TimeOfDay;

                        var workingTime = finishTime.Subtract(startTime);

                        //if (workingTime > WorkingRule.HalfWorkingHour && workingTime < WorkingRule.FullWorkingHour)
                        //{
                        //    if(startTime < WorkingRule.NoonHour)
                        //    {
                        //        workingReport.WorkingType = WorkingType.HalfDayMorning;
                        //    } else
                        //    {
                        //        workingReport.WorkingType = WorkingType.HalfDayAfternoon;
                        //    }
                        //}

                        if (workingTime < WorkingRule.FullWorkingHour)
                        {
                            workingReport.WorkingType = WorkingType.LackTime;
                            var minuteLate = WorkingRule.FullWorkingHour.Subtract(workingTime).TotalMinutes;
                            workingReport.MinuteLate = Math.Floor(minuteLate);
                        }
                        else
                        {
                            workingReport.WorkingType = WorkingType.FullWorkingDay;
                        }
                    }
                    return workingReport;

                }
                else
                {
                    workingReport.WorkingType = WorkingType.Absence;
                    workingReport.CheckIn = null;
                    workingReport.CheckOut = null;
                    return workingReport;
                }
            }

            return null;
        }

        public bool IsWorkingDay(DateTime day)
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

                var lackCheckoutRecords = dailyRecords.Where(d => d.WorkingType == WorkingType.LackCheckOut).ToList();
                reportRecords.AddRange(lackCheckoutRecords);

                var totalLackTime = dailyRecords.Sum(d => d.MinuteLate);
                if (totalLackTime > WorkingRule.TotalLackTime || lackCheckoutRecords.Count > 0)
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
}
