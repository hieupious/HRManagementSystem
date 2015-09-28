using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using HRMS.Web.Services;
using System.Diagnostics;

namespace HRMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImportDataService _importDataService;

        public HomeController(ApplicationDbContext dbContext, IImportDataService importDataService)
        {
            _dbContext = dbContext;
            _importDataService = importDataService;
        }
        public IActionResult Index()
        {
            //var users = _importDataService.ImportUserFromAccessDB();
            //var depts = _importDataService.ImportDepartmentFromAccessDB();
            Stopwatch stopwatch = Stopwatch.StartNew(); 
            var checkIns = _importDataService.ImportAllCheckInOutFromAccessDB();
            //_dbContext.Deparments.AddRange(depts);
            //_dbContext.UserInfoes.AddRange(users);
            _dbContext.CheckInOutRecords.AddRange(checkIns);
            _dbContext.SaveChanges();
            stopwatch.Stop();
            var user = _dbContext.UserInfoes.ToList();
            ViewBag.Time = stopwatch.ElapsedMilliseconds / 1000;
            ViewBag.Total = checkIns.Count;
            return View(user);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
