using FlexAirFit.Core.Enums;
using Microsoft.Extensions.Configuration;
using FlexAirFit.TechUI.BaseMenu;
using Serilog.Context;

namespace FlexAirFit.TechUI;

internal class Startup(IConfiguration config,
    Context context,
    List<Menu> menus)
{
    private readonly IConfiguration _config = config;
    private Context _context = context;

    private readonly Menu _guestMenu = menus[0];
    private readonly Menu _clientMenu = menus[1];
    private readonly Menu _adminMenu = menus[2];
    private readonly Menu _trainerMenu = menus[3];

    public async Task Run()
    {
        int choice;
        do
        {
            Console.WriteLine();
            if (_context.CurrentUser is null)
            {
                LogContext.PushProperty("UserRole", "Guest");
                Console.WriteLine("Статус пользователя: Гость");
                choice = await _guestMenu.Execute(_context);
            }
            else if (_context.CurrentUser.Role == UserRole.Client)
            {
                LogContext.PushProperty("UserRole", _context.CurrentUser.Role.ToString());
                LogContext.PushProperty("UserId", _context.CurrentUser.Id);
                Console.WriteLine($"Статус пользователя: Авторизованный клиент");
                choice = await _clientMenu.Execute(_context);
            }
            else if (_context.CurrentUser.Role == UserRole.Admin)
            {
                LogContext.PushProperty("UserRole", _context.CurrentUser.Role.ToString());
                LogContext.PushProperty("UserId", _context.CurrentUser.Id);
                Console.WriteLine($"Статус пользователя: Авторизованный администратор");
                choice = await _adminMenu.Execute(_context);
            }
            else 
            {
                LogContext.PushProperty("UserRole", _context.CurrentUser.Role.ToString());
                LogContext.PushProperty("UserId", _context.CurrentUser.Id);
                Console.WriteLine($"Статус пользователя: Авторизованный тренер");
                choice = await _trainerMenu.Execute(_context);
            }
        } while (choice != 0);
    }
}