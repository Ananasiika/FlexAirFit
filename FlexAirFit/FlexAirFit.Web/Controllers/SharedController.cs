using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace FlexAirFit.Web.Controllers;

public class SharedController : Controller
{
    private readonly Serilog.ILogger _logger = Log.ForContext<SharedController>();
    private readonly Context _context;

    public SharedController(ILogger<RegisterController> logger, Context context)
    {
        _context = context;
    }
    
    public IActionResult Return()
    {
        var userRole = Request.Cookies["UserRole"].ToString();

        switch (userRole)
        {
            case "Admin":
                return RedirectToAction("AdminDashboard", "Admin");
            case "Client":
                return RedirectToAction("ClientDashboard", "Client");
            case "Trainer":
                return RedirectToAction("TrainerDashboard", "Trainer");
            default:
                return RedirectToAction("Index", "Home");
        }
    }
}