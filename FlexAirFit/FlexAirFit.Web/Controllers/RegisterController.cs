using System.Globalization;
using System.Text.Json;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace FlexAirFit.Web.Controllers;

public class RegisterController : Controller
{
    private readonly ILogger _logger = Log.ForContext<RegisterController>();
    private readonly Context _context;

    public RegisterController(ILogger<RegisterController> logger, Context context)
    {
        _context = context;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserModel model)
    {
        _logger.Information("Executing Register action");

        if (ModelState.IsValid)
        {
            string password = model.Password;
            string passwordVerify = model.PasswordVerify;
            string email = model.Email;
            int numRole = model.RoleNumber;

            if (password != passwordVerify)
            {
                _logger.Warning("Passwords don't match");
                ModelState.AddModelError("PasswordVerify", "Пароли не совпадают.");
                return View(model);
            }

            UserRole role;
            switch (numRole)
            {
                case 1:
                    role = UserRole.Client;
                    break;
                case 2:
                    role = UserRole.Admin;
                    break;
                case 3:
                    role = UserRole.Trainer;
                    break;
                default:
                    _logger.Error("Invalid role number");
                    ModelState.AddModelError("RoleNumber", "Некорректный номер роли.");
                    return View(model);
            }

            var userId = Guid.NewGuid();
            var user = new User(userId, role, email, password);

            try
            {
                await _context.UserService.CreateUser(user, password, role);
                _logger.Information($"Created user: {user.Email}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error creating user: {ex.Message}");
                ModelState.AddModelError("", ex.Message);
                return View("Error", ex.Message);
            }

            // Создание соответствующей сущности в зависимости от роли
            switch (role)
            {
                case UserRole.Client:
                    var clientModel = new ClientModel
                    {
                        Name = Request.Form["Name"],
                        Gender = Request.Form["Gender"],
                        DateOfBirth = DateTime.Parse(Request.Form["ClientDateOfBirth"]),
                        IdMembership = Guid.Parse(Request.Form["MembershipId"])
                    };

                    var bonus = new Bonus(Guid.NewGuid(), userId, 0);
                    var client = new Core.Models.Client(userId, clientModel.Name, clientModel.Gender, clientModel.DateOfBirth.ToUniversalTime(), clientModel.IdMembership,
                        DateTime.Today.Add(_context.MembershipService.GetMembershipById(clientModel.IdMembership).Result.Duration), _context.MembershipService.GetMembershipById(clientModel.IdMembership).Result.Freezing, 
                        null);
                    try
                    {
                        await _context.ClientService.CreateClient(client);
                        await _context.BonusService.CreateBonus(bonus);
                        _logger.Information($"Created client: {client.Name}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"Error creating client: {ex.Message}");
                        ModelState.AddModelError("", ex.Message);
                        return View("Error", ex.Message);
                    }
                    break;

                case UserRole.Admin:
                    var adminModel = new AdminModel
                    {
                        Name = Request.Form["Name"],
                        Gender = Request.Form["Gender"],
                        DateOfBirth = DateTime.Parse(Request.Form["AdminDateOfBirth"])
                    };

                    var admin = new Core.Models.Admin(userId, adminModel.Name, adminModel.DateOfBirth.ToUniversalTime(), adminModel.Gender);
                    Console.WriteLine($"{userId}, {adminModel.Name}, {adminModel.DateOfBirth.ToUniversalTime()}, {adminModel.Gender}");
                    try
                    {
                        await _context.AdminService.CreateAdmin(admin);
                        _logger.Information($"Created admin: {admin.Name}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"Error creating admin: {ex.Message}");
                        ModelState.AddModelError("", ex.Message);
                        return View("Error", ex.Message);
                    }
                    break;

                case UserRole.Trainer:
                    var trainerModel = new TrainerModel
                    {
                        Name = Request.Form["Name"],
                        Gender = Request.Form["Gender"],
                        Specialization = Request.Form["Specialization"],
                        Experience = int.Parse(Request.Form["Experience"]),
                        Rating = int.Parse(Request.Form["Rating"])
                    };

                    var trainer = new Trainer(userId, trainerModel.Name, trainerModel.Gender, trainerModel.Specialization, trainerModel.Experience, trainerModel.Rating);
                    try
                    {
                        await _context.TrainerService.CreateTrainer(trainer);
                        _logger.Information($"Created trainer: {trainer.Name}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"Error creating trainer: {ex.Message}");
                        ModelState.AddModelError("", ex.Message);
                        return View("Error", ex.Message);
                    }
                    break;
            }
            return RedirectToAction("SuccessRegistration");
        }

        return View(model);
    }

    public IActionResult SuccessRegistration()
    {
        return View();
    }
}
