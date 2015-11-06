using System;
using Hangfire;
using HRMS.Web.Services;
using HRMS.Web.Models;
using HRMS.Web.Configuration;
using Microsoft.Framework.OptionsModel;

namespace HRMS.Web.Jobs
{
    public class RegisteredJob
    {
        private IImportDataService importDataService;
        private ApplicationDbContext dbContext;
        private ImportConfiguration importConfiguration;
        private IDailyWorkingProcessService dailyWorkingProcess;
        public RegisteredJob(IOptions<ImportConfiguration> importConfiguration,
            ApplicationDbContext dbContext, IDailyWorkingProcessService dailyWorkingProcess, IImportDataService importDataService)
        {
            this.importConfiguration = importConfiguration.Options;
            this.importDataService = importDataService;
            this.dailyWorkingProcess = dailyWorkingProcess;
            this.dbContext = dbContext;
        }

        public RegisteredJob()
        {

        }
        public void InitializeJobs()
        {
            var noonImport = importConfiguration.TimeToImportAtNoon;
            var nightImport = importConfiguration.TimeToImportAtNight;
            RecurringJob.AddOrUpdate<RegisteredJob>("noonImport", r => r.DailyCopyExternalFile(), Cron.Daily(noonImport.Hours, noonImport.Minutes), TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<RegisteredJob>("nightImport", r => r.DailyCopyExternalFile(), Cron.Daily(nightImport.Hours, nightImport.Minutes), TimeZoneInfo.Local);
        }

        public void MigrationJobs()
        {
            //BackgroundJob.Enqueue<RegisteredJob>(r => r.ImportAndProcessDataWithMonth(new DateTime(2015, 10, 1)));
            //BackgroundJob.Enqueue<RegisteredJob>(r => r.ImportAndProcessDataWithDay());
            //ProcessDailyWorkingReportJobWithDay(new DateTime(2015, 11, 6));
        }

        public void DailyCopyExternalFile()
        {
            var lastWriteTime = DateTime.Now;
            var copySuccessful = importDataService.CopyFileFromExternal(ref lastWriteTime);
            if (copySuccessful)
            {
                // write down latest import time.      
                ProcessDailyWorkingReportJobWithDay(DateTime.Now);
            }
        }

        public void DailyImportJob()
        {
            importDataService.ImportCheckInOutFromAccessDBDaily();
        }

        public void ImportDataJobWithDay(DateTime day)
        {
            importDataService.ImportCheckInOutRecordWithDay(day);
        }

        public void ProcessDailyWorkingReportJobWithDay(DateTime day)
        {
            dailyWorkingProcess.ProcessDailyWorkingReport(day);
        }

        public void ImportAndProcessDataWithMonth(DateTime month)
        {
            foreach (var day in WorkingProcessService.AllDatesInMonth(month.Year, month.Month))
            {
                if (day.Date <= DateTime.Now.Date)
                {
                    ProcessDailyWorkingReportJobWithDay(day);
                }
            }

        }

        public void ImportAndProcessDataWithDay(DateTime fromDay, DateTime toDay)
        {
            foreach (var day in WorkingProcessService.DaysBetweenTwoDays(fromDay, toDay))
            {
                ProcessDailyWorkingReportJobWithDay(day);
            }
        }



        public void BootstapData()
        {
            var _standard_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
            {
                WorkingTimeStart = new TimeSpan(8, 0, 0),
                WorkingTimeEnd = new TimeSpan(17, 0, 0),
                BreaktimeStart = new TimeSpan(12, 0, 0),
                BreaktimeEnd = new TimeSpan(13, 0, 0)
            };

            var _standard_ml_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
            {
                WorkingTimeStart = new TimeSpan(9, 0, 0),
                WorkingTimeEnd = new TimeSpan(17, 0, 0),
                BreaktimeStart = new TimeSpan(12, 0, 0),
                BreaktimeEnd = new TimeSpan(13, 0, 0)
            };

            var _standard_me_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
            {
                WorkingTimeStart = new TimeSpan(8, 0, 0),
                WorkingTimeEnd = new TimeSpan(16, 0, 0),
                BreaktimeStart = new TimeSpan(12, 0, 0),
                BreaktimeEnd = new TimeSpan(13, 0, 0)
            };

            var _devsqas_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
            {
                WorkingTimeStart = new TimeSpan(8, 0, 0),
                WorkingTimeEnd = new TimeSpan(17, 0, 0),
                BreaktimeStart = new TimeSpan(11, 45, 0),
                BreaktimeEnd = new TimeSpan(12, 45, 0)
            };

            var _devsqas_EarlyToleranceWorkingHoursRule = new EarlyToleranceWorkingHoursRule()
            {
                Tolerance = new TimeSpan(1, 0, 0)
            };

            var _devsqas_LateToleranceWorkingHoursRule = new LateToleranceWorkingHoursRule()
            {
                Tolerance = new TimeSpan(1, 0, 0)
            };

            var _devsqas_ml_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
            {
                WorkingTimeStart = new TimeSpan(9, 0, 0),
                WorkingTimeEnd = new TimeSpan(17, 0, 0),
                BreaktimeStart = new TimeSpan(11, 45, 0),
                BreaktimeEnd = new TimeSpan(12, 45, 0)
            };

            var _devsqas_me_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
            {
                WorkingTimeStart = new TimeSpan(8, 0, 0),
                WorkingTimeEnd = new TimeSpan(16, 0, 0),
                BreaktimeStart = new TimeSpan(11, 45, 0),
                BreaktimeEnd = new TimeSpan(12, 45, 0)
            };

            var _special2_LateToleranceWorkingHoursRule = new LateToleranceWorkingHoursRule()
            {
                Tolerance = new TimeSpan(0, 30, 0)
            };

            dbContext.BaseTimeWorkingHoursRules.Add(_standard_BaseTimeWorkingHoursRule);
            dbContext.BaseTimeWorkingHoursRules.Add(_standard_ml_BaseTimeWorkingHoursRule);
            dbContext.BaseTimeWorkingHoursRules.Add(_standard_me_BaseTimeWorkingHoursRule);
            dbContext.BaseTimeWorkingHoursRules.Add(_devsqas_BaseTimeWorkingHoursRule);
            dbContext.BaseTimeWorkingHoursRules.Add(_devsqas_me_BaseTimeWorkingHoursRule);
            dbContext.BaseTimeWorkingHoursRules.Add(_devsqas_ml_BaseTimeWorkingHoursRule);
            dbContext.EarlyToleranceWorkingHoursRules.Add(_devsqas_EarlyToleranceWorkingHoursRule);
            dbContext.LateToleranceWorkingHoursRules.Add(_devsqas_LateToleranceWorkingHoursRule);
            dbContext.LateToleranceWorkingHoursRules.Add(_special2_LateToleranceWorkingHoursRule);

            var stdEmp = new WorkingPoliciesGroup()
            {
                Name = "Standard Employee"
            };
            var stdEmpML = new WorkingPoliciesGroup()
            {
                Name = "Standard Employee Maternity Arrive Late"
            };
            var stdEmpME = new WorkingPoliciesGroup()
            {
                Name = "Standard Employee Maternity Leave Early"
            };
            var devQA = new WorkingPoliciesGroup()
            {
                Name = "Devs/QAs"
            };
            var devQAML = new WorkingPoliciesGroup()
            {
                Name = "Devs/QAs Maternity Arrive Late"
            };
            var devQAME = new WorkingPoliciesGroup()
            {
                Name = "Devs/QAs Maternity Leave Early"
            };
            var special_1 = new WorkingPoliciesGroup()
            {
                Name = "Special 1"
            };
            var special_1_ML = new WorkingPoliciesGroup()
            {
                Name = "Special 1 Maternity Arrive Late"
            };
            var special_1_ME = new WorkingPoliciesGroup()
            {
                Name = "Special 1 Maternity Leave Early"
            };

            dbContext.WorkingPoliciesGroups.AddRange(new WorkingPoliciesGroup[] { stdEmp, stdEmpME, stdEmpML, devQA, devQAME, devQAML, special_1, special_1_ME, special_1_ML });
            dbContext.SaveChanges();
        }
    }
}

