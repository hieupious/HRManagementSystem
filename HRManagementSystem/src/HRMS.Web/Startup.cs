using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Data.Entity;
using HRMS.Web.Models;
using HRMS.Web.Services;
using Hangfire;
using HRMS.Web.Configuration;
using HRMS.Web.Jobs;
using Hangfire.SqlServer;

namespace HRMS.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                            .AddJsonFile("config.json")
                            .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("NormalUser", policy => policy.RequireRole("NormalUser"));
                options.AddPolicy("HRGroup", policy => policy.RequireRole("HRGroup"));
                options.AddPolicy("Cookie", policy =>
                {
                    policy.ActiveAuthenticationSchemes.Add("Cookie");
                    policy.RequireAuthenticatedUser();
                });
            });


            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            // Add application service
            services.AddTransient<IImportDataService, ImportDataService>();
            services.AddTransient<IDailyWorkingProcessService, WorkingProcessService>();
            services.AddTransient<IMonthlyWorkingProcess, WorkingProcessService>();
            services.AddTransient<IWorkingHoursValidator, WorkingHoursValidator>();
            services.AddTransient<RegisteredJob, RegisteredJob>();
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

            app.UseCookieAuthentication(options =>
            {
                options.AuthenticationScheme = "Cookie";
                options.LoginPath = "/Home/SignIn";
                options.AccessDeniedPath = "/Home/PermissionDenied";
                options.AutomaticAuthentication = true;
            });

            var hangfireOptions = new SqlServerStorageOptions()
            {
                PrepareSchemaIfNecessary = true

            };
            // Configure for Hangfire Server
            GlobalConfiguration.Configuration.UseSqlServerStorage(Configuration["Data:HangfireServer:ConnectionString"], hangfireOptions);
            GlobalConfiguration.Configuration.UseActivator(new ServiceJobActivator(app.ApplicationServices));

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                AuthorizationFilters = new[] { new HangfireAuthorizationFilter() }
            });

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            app.UseIdentity();

            //app.ApplicationServices.GetService<RegisteredJob>().InitializeJobs();
            app.ApplicationServices.GetService<RegisteredJob>().MigrationJobs();
        }
    }
}
