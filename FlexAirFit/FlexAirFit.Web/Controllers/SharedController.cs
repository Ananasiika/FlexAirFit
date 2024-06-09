using FlexAirFit.Application.Exceptions.ServiceException;
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
    
    public IActionResult Error()
    {
        string errorType = Request.Cookies["errorType"].ToString();
        Response.Cookies.Delete("errorType");
        switch (errorType)
        {
            case "WorkoutExistsException":
                return View("Error", "Тренировка уже существует");
            case "WorkoutNotFoundException":
                return View("Error", "Тренировка не найдена");
            case "UserNotFoundException":
                return View("Error", "Пользователь не найден");
            case "UserExistsException":
                return View("Error", "Пользователь уже существует");
            case "AdminNotFoundException":
                return View("Error", "Администратор не найден");
            case "AdminExistsException":
                return View("Error", "Администратор уже существует");
            case "TrainerNotFoundException":
                return View("Error", "Тренер не найден");
            case "TrainerExistsException":
                return View("Error", "Тренер уже существует");
            case "ClientNotFoundException":
                return View("Error", "Клиент не найден");
            case "ClientExistsException":
                return View("Error", "Клиент уже существует");
            case "ProductNotFoundException":
                return View("Error", "Товар не найден");
            case "ProductExistsException":
                return View("Error", "Товар уже существует");
            case "ScheduleNotFoundException":
                return View("Error", "Такая запись в расписании не найдена");
            case "ScheduleExistsException":
                return View("Error", "Такая запись в расписании уже существует");
            case "MembershipExistsException":
                return View("Error", "Абонемент уже существует");
            case "MembershipNotFoundException":
                return View("Error", "Абонемент не найден");
            case "InvalidFreezingException":
                return View("Error", "Неверные данные заморозки");
            case "ScheduleTimeIncorrectedException":
                return View("Error", "Время тренировки некорректно");
            case "TrainerAlreadyHasScheduleException":
                return View("Error", "Тренер уже имеет тренировку в это время");
            case "ClientAlreadyHasScheduleException":
                return View("Error", "Клиент уже имеет тренировку в это время");
            case "ClientNotActiveException":
                return View("Error", "Абонемент клиента не активен");
            default:
                return View("Error", "Произошла ошибка");
        }
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