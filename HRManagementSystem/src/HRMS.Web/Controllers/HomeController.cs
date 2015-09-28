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
            return View();
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
