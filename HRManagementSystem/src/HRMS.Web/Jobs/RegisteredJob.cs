using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using HRMS.Web.Services;

namespace HRMS.Web.Jobs
{
    public class RegisteredJob
    {
        public RegisteredJob()
        {

        }

        public static void InitializeJobs()
        {
            //BackgroundJob.Enqueue<ImportDataService>(i => i.CopyFileFromExternal());
        }
    }
}
