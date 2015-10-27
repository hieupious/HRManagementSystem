using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using HRMS.Web.Services;
using HRMS.Web.Models;
using HRMS.Web.Configuration;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;


namespace HRMS.Web.Jobs
{
    public class RegisteredJob
    {
        private IImportDataService importDataService;
        private ApplicationDbContext dbContext;
        private ImportConfiguration importConfiguration;
        private IDailyWorkingProcessService dailyWorkingProcess;
        IServiceProvider app;
        public RegisteredJob(IOptions<ImportConfiguration> importConfiguration,
            IServiceProvider app, ApplicationDbContext dbContext)
        {
            this.app = app;
            this.importConfiguration = importConfiguration.Options;
            importDataService = app.GetService<IImportDataService>();
            dailyWorkingProcess = app.GetService<IDailyWorkingProcessService>();
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
            //BackgroundJob.ContinueWith<RegisteredJob>("1", r => r.DailyImportJob(), JobContinuationOptions.OnlyOnSucceededState);
            RecurringJob.AddOrUpdate<RegisteredJob>("nightImport", r => r.DailyCopyExternalFile(), Cron.Daily(nightImport.Hours, nightImport.Minutes), TimeZoneInfo.Local);
            //BackgroundJob.ContinueWith<RegisteredJob>("2", r => r.DailyImportJob(), JobContinuationOptions.OnlyOnSucceededState);
        }

        public void MigrationJobs()
        {
            BackgroundJob.Enqueue<RegisteredJob>(r => r.ImportAndHandleData());
        }

        public void DailyCopyExternalFile()
        {
            var lastWriteTime = DateTime.Now;
            var copySuccessful = importDataService.CopyFileFromExternal(ref lastWriteTime);
            if (copySuccessful)
            {
                dbContext = app.GetService<ApplicationDbContext>();
                // write down latest import time.                
                var result = importDataService.ImportCheckInOutRecordWithDay(DateTime.Now, null, dbContext);
                if (result > 0)
                {
                    ProcessDailyWorkingReportJobWithDay(DateTime.Now);
                }
                dbContext.Dispose();
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
            var userIds = dbContext.UserInfoes.ToList();
            foreach (var userId in userIds)
            {
                dailyWorkingProcess.ProcessDailyWorkingReport(userId.Id, day, dbContext);
            }
        }

        public void ProcessDailyWorkingReportJob()
        {
            ProcessDailyWorkingReportJobWithDay(DateTime.Now);
        }

        public void ImportAndHandleData()
        {
            foreach (var day in WorkingProcessService.AllDatesInMonth(2015, 10))
            {
                if (day.Date < DateTime.Now.Date)
                {
                    
                    dbContext = app.GetService<ApplicationDbContext>();

                    var result = importDataService.ImportCheckInOutRecordWithDay(day, null, dbContext);
                    if (result > 0)
                    {
                        ProcessDailyWorkingReportJobWithDay(day);
                    }
                    dbContext.Dispose();

                }
            }

        }



        public void ProcessMonthlyReport(DateTime month)
        {

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

