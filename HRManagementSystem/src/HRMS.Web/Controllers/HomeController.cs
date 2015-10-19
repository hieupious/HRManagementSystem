using System.Linq;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using HRMS.Web.Services;
using Microsoft.Data.Entity;
using HRMS.Web.Configuration;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace HRMS.Web.Controllers
{

    public class HomeController : Controller
    {
        private IOptions<ImportConfiguration> importConfig;
        private readonly ApplicationDbContext dbContext;
        private readonly IImportDataService importDataService;
        private readonly IDailyWorkingProcessService dailyWorkingProcess;
        private readonly IMonthlyWorkingProcess monthlyWorkingProcess;
        private static int[] activeDepts = { 2, 3, 6, 7, 8, 9, 10 };

        public HomeController(ApplicationDbContext dbContext,
            IImportDataService importDataService, IDailyWorkingProcessService dailyWorkingProcess,
            IOptions<ImportConfiguration> importConfig, IMonthlyWorkingProcess monthlyWorkingProcess)
        {
            this.dbContext = dbContext;
            this.importDataService = importDataService;
            this.dailyWorkingProcess = dailyWorkingProcess;
            this.monthlyWorkingProcess = monthlyWorkingProcess;
            this.importConfig = importConfig;
        }
        [Authorize(Roles = "NormalUser,Manager,Administrator,HRGroup")]
        public IActionResult Index(string id)
        {
            if (User.IsInRole("NormalUser"))
            {
                return RedirectToAction(nameof(HomeController.UserInfo), "Home");
            }
            ViewBag.SearchTerms = id;
            return View();
        }

        public async Task<IActionResult> SignIn()
        {
            if (WindowsIdentity.GetCurrent().IsAuthenticated) // check if user signed in, sign them in else redirect to error page.
            {
                var windowsAccountName = WindowsIdentity.GetCurrent().Name;
                // base Identity Name >> get user >> check if user valid >> get Name, get Role, register
                var user = dbContext.UserInfoes.FirstOrDefault(u => string.Equals(u.WindowsAccount, windowsAccountName, System.StringComparison.CurrentCultureIgnoreCase));
                if (user != null)
                {
                    var role = user.Role.ToString();
                    var issuer = "hrms.aswig";
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String, issuer));
                    claims.Add(new Claim(ClaimTypes.Sid, user.EmployeeId));
                    claims.Add(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, issuer));

                    var identity = new ClaimsIdentity(claims, "hrmsAuth");

                    var principal = new ClaimsPrincipal(identity);
                    await Context.Authentication.SignInAsync("Cookie", principal);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        public async Task<IActionResult> SignOut()
        {
            await Context.Authentication.SignOutAsync("Cookie");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize(Roles = "NormalUser,Manager,Administrator,HRGroup")]
        public IActionResult UserInfo(int? id, string searchTerms)
        {
            if (!id.HasValue)
                id = int.Parse(User.FindFirstValue(ClaimTypes.Sid));


            var user = dbContext.UserInfoes.Include(u => u.Department).Where(u => int.Parse(u.EmployeeId) == id).FirstOrDefault();
            ViewBag.SearchTerms = searchTerms;
            return View(user);
        }

        [Authorize(Roles = "HRGroup")]
        public IActionResult Report()
        {
            return View();
        }

        [Authorize(Roles = "Manager,HRGroup")]
        public IActionResult PendingApprovals()
        {
            return View();
        }

        public IActionResult Error()
        {

            return View("~/Views/Shared/Error.cshtml");
        }

    }
}
