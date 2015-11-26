using System.Linq;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using HRMS.Web.ViewModels;
using System;
using HRMS.Web.Services;
using AutoMapper;
using AutoMapper.Mappers;
namespace HRMS.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private readonly IWorkingHoursValidator workingHoursValidator;

        private DbSet<WorkingPoliciesGroup> _policies;

        private DbSet<WorkingPoliciesGroup> WorkingPoliciesGroup
        {
            get
            {
                if (_policies == null)
                {
                    _policies = dbContext.GetWorkingPoliciesGroups();
                }
                return _policies;
            }
        }

        public HomeController(ApplicationDbContext dbContext, IWorkingHoursValidator workingHoursValidator)
        {
            this.dbContext = dbContext;
            this.workingHoursValidator = workingHoursValidator;
        }

        [Authorize(Roles = "NormalUser,Manager,Administrator,HRGroup")]
        public IActionResult Index(string id)
        {
            if (User.IsInRole("NormalUser"))
            {
                return RedirectToAction(nameof(HomeController.Dashboard), "Home");
            }
            ViewBag.SearchTerms = id;
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignIn()
        {
            if (WindowsIdentity.GetCurrent().IsAuthenticated) // check if user signed in, sign them in else redirect to error page.
            {
                var windowsAccountName = User.GetUserName() ?? WindowsIdentity.GetCurrent().Name;
                // base Identity Name >> get user >> check if user valid >> get Name, get Role, register
                var user = dbContext.UserInfoes.FirstOrDefault(u => string.Equals(u.WindowsAccount, windowsAccountName, System.StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    var role = user.Role.ToString();
                    var issuer = "hrms.aswig";
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String, issuer));
                    claims.Add(new Claim(ClaimTypes.Sid, user.EmployeeId));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, issuer));

                    var identity = new ClaimsIdentity(claims, "hrmsAuth");

                    var principal = new ClaimsPrincipal(identity);
                    await Context.Authentication.SignInAsync("Cookie", principal);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            return RedirectToAction(nameof(HomeController.PermissionDenied), "Home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignOut()
        {
            await Context.Authentication.SignOutAsync("Cookie");
            return new HttpUnauthorizedResult();
        }

        [Authorize(Roles = "NormalUser,Manager,Administrator,HRGroup")]
        public IActionResult UserInfo(int? id, string searchTerms)
        {
            if (!id.HasValue || User.IsInRole("NormalUser"))
                id = int.Parse(User.FindFirstValue(ClaimTypes.Sid));

            ViewBag.ShowApproval = false;
            var user = dbContext.UserInfoes.Include(u => u.Department).Where(u => int.Parse(u.EmployeeId) == id).FirstOrDefault();
            if (user.Id == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                ViewBag.ShowApproval = true;
            }

            ViewBag.SearchTerms = searchTerms;
            return View(user);
        }

        [Authorize(Roles = "Administrator,HRGroup")]
        public IActionResult Report()
        {
            return View();
        }

        [Authorize(Roles = "Administrator,HRGroup")]
        public IActionResult PublicHoliday()
        {
            return View();
        }

        public IActionResult Dashboard()
        {

            string id = User.FindFirstValue(ClaimTypes.Sid);
            var user = dbContext.UserInfoes.Include(u => u.WorkingPoliciesGroup).Include(u => u.Department).FirstOrDefault(u => u.EmployeeId.Equals(id));
            if (user == null)
                return RedirectToAction(nameof(HomeController.PermissionDenied), "Home");

            var record = dbContext.DailyWorkingRecords.Include(d => d.CheckInOutRecords).FirstOrDefault(d => d.WorkingDay.Date == DateTime.Now.Date && d.UserInfoId == user.Id);

            UserInfoViewModel viewModel = new UserInfoViewModel
            {
                Name = user.Name,
                EmployeeId = user.EmployeeId,
                Department = user.Department.Name,
                Office = user.Office.ToString(),
                TotalLackingTimeInMonth = CaculateTotalLackingTimeInMonth(DateTime.Now).ToString(),
                WorkingHourRuleApplied = user.WorkingPoliciesGroup.Name,
                CurrentDayCheckinTime = record != null && record.CheckIn != null ? record.CheckIn.Value.ToShortTimeString() : string.Empty
            };

            if (record != null && record.CheckIn != null)
            {
                viewModel.WorkingHourRuleApplied = record.CheckIn.Value.ToShortTimeString();
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Manager,Administrator,HRGroup")]
        public IActionResult PendingApprovals()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        [AllowAnonymous]
        public IActionResult PermissionDenied()
        {
            return View("~/Views/Shared/PermissionDenied.cshtml");
        }

        #region Help methods
        private int CaculateTotalLackingTimeInMonth(DateTime? currentDate)
        {
            if (currentDate == null)
            {
                currentDate = DateTime.Now;
            }

            string id = User.FindFirstValue(ClaimTypes.Sid);
            var user = dbContext.UserInfoes.FirstOrDefault(u => u.EmployeeId.Equals(id));
            if (user.WorkingPoliciesGroupId.HasValue)
            {
                user.WorkingPoliciesGroup = WorkingPoliciesGroup.SingleOrDefault(w => w.Id == user.WorkingPoliciesGroupId.Value);
            }

            int totalLackingTime = 0;
            foreach (var day in WorkingProcessService.AllDatesInMonth(currentDate.Value.Year, currentDate.Value.Month))
            {
                var record = dbContext.DailyWorkingRecords.Include(d => d.CheckInOutRecords).FirstOrDefault(d => d.WorkingDay.Date == day.Date && d.UserInfoId.Value == user.Id);
                if (record != null)
                {
                    totalLackingTime += workingHoursValidator.ValidateDailyRecord(record, user.WorkingPoliciesGroup, day);
                }
            }

            return totalLackingTime;
        }
        #endregion
    }
}
