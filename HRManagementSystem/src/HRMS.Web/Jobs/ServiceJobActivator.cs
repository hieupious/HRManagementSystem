using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Jobs
{
    public class ServiceJobActivator: JobActivator
    {
        private IServiceProvider provider;

        public ServiceJobActivator(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public override object ActivateJob(Type jobType)
        {
            return provider.GetService(jobType) ?? Activator.CreateInstance(jobType);
        }
    }
}
