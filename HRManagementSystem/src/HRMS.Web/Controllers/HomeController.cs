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

        public IActionResult Index(string id)
        {
            ViewBag.SearchTerms = id;
            return View();
        }

        public IActionResult UserInfo(int id, string searchTerms)
        {
            var model = new UserInfo()
            {
                Id = 223,
                Name = "Tran Quoc Linh",
                Department = new Department()
                {
                    Id = 7,
                    Name = "2XX GP OFFICE"
                }
            };

            ViewBag.SearchTerms = searchTerms;
            return View(model);
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
