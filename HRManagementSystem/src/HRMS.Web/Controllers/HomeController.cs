using System.Linq;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace HRMS.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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

    }
}
