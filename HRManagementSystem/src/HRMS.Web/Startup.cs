﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Data.Entity;
using HRMS.Web.Models;
using HRMS.Web.Services;
using Autofac;
//using Autofac.Dnx;
using Hangfire;

using HRMS.Web.Configuration;

namespace HRMS.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                            .AddJsonFile("config.json");
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            appBasePath = appEnv.ApplicationBasePath;
        }

        public string appBasePath { get; set; }
        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration["Data:DefaultConnection:ConnectionString"];
            services.AddEntityFramework()
                            .AddSqlServer()
                            .AddDbContext<ApplicationDbContext>(options =>
                                options.UseSqlServer(connection));
            // Add MVC services to the services container.
            services.AddMvc();

            // Add Configuration service
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<ImportConfiguration>(Configuration.GetSection("ImportConfiguration"));
            services.Configure<ImportConfiguration>(options =>
            {
                options.ApplicationBasePath = appBasePath;
            });


            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            // Add application service
            services.AddTransient<IImportDataService, ImportDataService>();
            services.AddTransient<IDailyWorkingProcessService, WorkingProcessService>();
            services.AddTransient<IMonthlyWorkingProcess, WorkingProcessService>();
            JobActivator.Current = new ServiceJobActivator(services);
            //var dbPath = appBasePath + "\\" + Configuration["Data:ImportedDBPath:dbPath"];
            //var builder = new ContainerBuilder();
            //builder.Register(svc => new ImportDataService()).As<IImportDataService>().InstancePerLifetimeScope();
            //builder.Register(svc => new WorkingProcessService()).As<IDailyWorkingProcessService>().InstancePerLifetimeScope();
            //builder.Register(svc => new WorkingProcessService()).As<IMonthlyWorkingProcess>().InstancePerLifetimeScope();
            //Populate the container with services that were previously registered
            //builder.Populate(services);
            //GlobalConfiguration.Configuration.UseAutofacActivator(builder.Build());

            //GlobalConfiguration.Configuration.UseActivator(new ServiceJobActivator(services));
            //var container = builder.Build();

            //return container.Resolve<IServiceProvider>();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Configure the HTTP request pipeline.

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseErrorPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Configure for Hangfire Server
            GlobalConfiguration.Configuration.UseSqlServerStorage(Configuration["Data:DefaultConnection:ConnectionString"]);
            
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapWebApiRoute(
                //    name: "UserReportApi",
                //    template: "api/{controller}/{empId}/{action}/{month?}",
                //    defaults: null,
                //    constraints: new
                //    {
                //        controller = "Users",
                //        action = "Report"
                //    });

                routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
            
        }
    }

    public class ServiceJobActivator : JobActivator
    {
        private IServiceCollection _service;

        public ServiceJobActivator(IServiceCollection service)
        {
            _service = service;            
        }

        public override object ActivateJob(Type type)
        {
            return _service.AddInstance(type);
        }
    }

    //public class ContainerJobActivator : JobActivator
    //{
    //    private IContainer _container;

    //    public ContainerJobActivator(IContainer container)
    //    {
    //        _container = container;
    //    }

    //    public override object ActivateJob(Type type)
    //    {
    //        return _container.Resolve(type);
    //    }
    //}
}
