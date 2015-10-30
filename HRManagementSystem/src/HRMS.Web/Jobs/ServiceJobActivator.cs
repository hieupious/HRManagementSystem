using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
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

    public class HangfireAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            var context = new OwinContext(owinEnvironment);
            return context.Authentication.User.Identity.IsAuthenticated && context.Authentication.User.IsInRole("Administrator");
        }
    }
}
