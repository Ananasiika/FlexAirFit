using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace FlexAirFit.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Response.Cookies.Append("UserId", Guid.Empty.ToString());
            Response.Cookies.Append("UserRole", "Guest");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
