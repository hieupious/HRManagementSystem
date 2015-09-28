using Microsoft.AspNet.Mvc;
using HRMS.Models;

namespace HRMS.Controllers
{
    public class HomeController : Controller
    {
        private Repository _repository;
        public HomeController(Repository repository)
        {
            _repository = repository;
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
