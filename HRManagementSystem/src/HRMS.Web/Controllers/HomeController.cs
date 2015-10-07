using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using HRMS.Web.Services;
using System.Diagnostics;
using Microsoft.Data.Entity;
using HRMS.Web.Configuration;
using Microsoft.Framework.OptionsModel;
using Hangfire;

namespace HRMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private IOptions<AppSettings> appSetting;
        private IOptions<ImportConfiguration> importConfig;
        private readonly ApplicationDbContext dbContext;
        private readonly IImportDataService importDataService;
        private readonly IDailyWorkingProcessService dailyWorkingProcess;

        public HomeController(ApplicationDbContext dbContext, 
            IImportDataService importDataService, IDailyWorkingProcessService dailyWorkingProcess, IOptions<ImportConfiguration> importConfig)
        {
            this.dbContext = dbContext;
            this.importDataService = importDataService;
            this.dailyWorkingProcess = dailyWorkingProcess;
            this.importConfig = importConfig;
        }

        public IActionResult Index(string id)
        {
            ViewBag.SearchTerms = id;
            return View();
        }

        public IActionResult UserInfo(int id, string searchTerms)
        {
            var user = dbContext.UserInfoes.Include(u => u.Department).Where(u => int.Parse(u.EmployeeId) == id).FirstOrDefault();
            ViewBag.SearchTerms = searchTerms;
            return View(user);
        }

        public IActionResult Report()
        {
            return View();
        }

        public IActionResult Error()
        {
            
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
