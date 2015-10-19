using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public RegisteredJob(IImportDataService importDataService, ApplicationDbContext dbContext,
            IOptions<ImportConfiguration> importConfiguration)
        {
            this.importDataService = importDataService;
            this.dbContext = dbContext;
            this.importConfiguration = importConfiguration.Options;
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

        public void DailyCopyExternalFile()
        {
            var lastWriteTime = DateTime.Now;
            var copySuccessful = importDataService.CopyFileFromExternal(ref lastWriteTime);
            if (copySuccessful)
            {
                importDataService.ImportDailyCheckInOutFromAccessDB();                
            }
        }
    }
}

