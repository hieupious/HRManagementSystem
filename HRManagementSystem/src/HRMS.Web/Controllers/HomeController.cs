using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using HRMS.Web.Services;
using System.Diagnostics;
using Microsoft.Data.Entity;

namespace HRMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImportDataService _importDataService;
        private readonly IDailyWorkingProcessService _dailyWorkingProcess;

        public HomeController(ApplicationDbContext dbContext, IImportDataService importDataService, IDailyWorkingProcessService dailyWorkingProcess)
        {
            _dbContext = dbContext;
            _importDataService = importDataService;
            _dailyWorkingProcess = dailyWorkingProcess;
        }

        public IActionResult Index(string id)
        {
            ViewBag.SearchTerms = id;
            return View();
        }

        public IActionResult UserInfo(int id, string searchTerms)
        {
            var user = _dbContext.UserInfoes.Include(u => u.Department).Where(u => int.Parse(u.EmployeeId) == id).FirstOrDefault();
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
