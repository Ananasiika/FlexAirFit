using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using ILogger = Serilog.ILogger;

namespace FlexAirFit.Web.Controllers;

public class LoginController : Controller
{
    private readonly ILogger _logger = Log.ForContext<RegisterController>();
    private readonly Context _context;

    public LoginController(ILogger<RegisterController> logger, Context context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserModel model)
    {
        _logger.Information("Executing Login action");

        if (model.Email is null && model.Password is null)
        {
            _logger.Error("Invalid input");
            return View(model);
        }

        try
        {
            Console.WriteLine("User logged in successfully");
            var user = await _context.UserService.SignInUser(model.Email!, model.Password!);
            _context.CurrentUser = user;
            Response.Cookies.Append("UserId", user.Id.ToString());
            Response.Cookies.Append("UserRole", user.Role.ToString());
            LogContext.PushProperty("UserRole", _context.CurrentUser.Role.ToString());
            LogContext.PushProperty("UserId", _context.CurrentUser.Id);
            _logger.Information("User logged in successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            _logger.Error(ex.Message);
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
        
        switch (_context.CurrentUser.Role.ToString())
        {
            case "Admin":
                return RedirectToAction("AdminDashboard", "Admin");
            case "Client":
                return RedirectToAction("ClientDashboard", "Client");
            case "Trainer":
                return RedirectToAction("TrainerDashboard", "Trainer");
            default:
                return View(model);
        }
    }

    public IActionResult SuccessLogin()
    {
        return View();
    }
    
    public ActionResult Logout()
    {
        _context.CurrentUser = null;
        return RedirectToAction("Index", "Home");
    }

}
