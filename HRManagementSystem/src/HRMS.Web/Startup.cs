using System;
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
using Autofac.Dnx;


namespace HRMS.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.
            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
            appBasePath = appEnv.ApplicationBasePath;
        }

        public string appBasePath { get; set; }
        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration["Data:DefaultConnection:ConnectionString"];
            services.AddEntityFramework()
                            .AddSqlServer()
                            .AddDbContext<ApplicationDbContext>(options =>
                                options.UseSqlServer(connection));
            // Add MVC services to the services container.
            services.AddMvc();

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            //services.AddWebApiConventions();

            // Add application service
            //services.AddTransient<IImportDataService, ImportDataService>();
            var dbPath = appBasePath + "\\" + Configuration["Data:ImportedDBPath:dbPath"];
            var builder = new ContainerBuilder();
            builder.Register(svc => new ImportDataService(dbPath)).As<IImportDataService>().InstancePerLifetimeScope();
            builder.Register(svc => new WorkingProcessService()).As<IDailyWorkingProcessService>().InstancePerLifetimeScope();
            //Populate the container with services that were previously registered
            builder.Populate(services);

            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
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

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }
    }
}
