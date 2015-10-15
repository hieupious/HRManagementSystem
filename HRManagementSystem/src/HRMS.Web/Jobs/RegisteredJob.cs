using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using HRMS.Web.Services;
using HRMS.Web.Models;

namespace HRMS.Web.Jobs
{
    public class RegisteredJob
    {
        private IImportDataService importDataService;
        private ApplicationDbContext dbContext;
        public RegisteredJob(IImportDataService importDataService, ApplicationDbContext dbContext)
        {
            this.importDataService = importDataService;
            this.dbContext = dbContext;
        }

        public RegisteredJob()
        {

        }
        public static void InitializeJobs()
        {
            RecurringJob.AddOrUpdate<RegisteredJob>(r => r.DailyCopyExternalFile(), Cron.Daily(22, 0), TimeZoneInfo.Local);
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

