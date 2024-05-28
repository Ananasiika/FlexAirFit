using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace FlexAirFit.Web.Controllers;

public class AdminController : Controller
{
    private readonly Serilog.ILogger _logger = Log.ForContext<AdminController>();
    private readonly Context _context;

    public AdminController(ILogger<ProductController> logger, Context context)
    {
        _context = context;
    }
    public IActionResult AdminDashboard()
    {
        return View();
    }

}