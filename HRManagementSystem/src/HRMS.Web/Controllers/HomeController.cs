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
        private readonly IDailyWorkingProcessService _dailyWorkingProcess;

        public HomeController(ApplicationDbContext dbContext, IImportDataService importDataService, IDailyWorkingProcessService dailyWorkingProcess)
        {
            _dbContext = dbContext;
            _importDataService = importDataService;
            _dailyWorkingProcess = dailyWorkingProcess;
        }
        public IActionResult Index()
        {
            //var checkInOutRecords = _dbContext.CheckInOutRecords.Where(c => c.CheckTime.Date == new DateTime(2005, 8, 3)).ToList();
            //int count = checkInOutRecords.Count;
            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
