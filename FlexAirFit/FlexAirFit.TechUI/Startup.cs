using FlexAirFit.Core.Enums;
using Microsoft.Extensions.Configuration;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI;

internal class Startup(IConfiguration config,
    Context context,
    List<Menu> menus)
{
    private readonly IConfiguration _config = config;
    private Context _context = context;

    private readonly Menu _guestMenu = menus[0];

    public async Task Run()
    {
        int choice;
        do
        {
            Console.WriteLine();
            if (_context.CurrentUser is null)
            {
                Console.WriteLine("Статус пользователя: Гость");
                choice = await _guestMenu.Execute(_context);
            }
            else if (_context.CurrentUser.Role == UserRole.Client)
            {
                Console.WriteLine($"Статус пользователя: Авторизованный Client");
                choice = await _clientMenu.Execute(_context);
            }
            else if (_context.CurrentUser.Role == UserRole.Admin)
            {
                Console.WriteLine($"Статус пользователя: Авторизованный Admin");
                choice = await _adminMenu.Execute(_context);
            }
            else 
            {
                Console.WriteLine($"Статус пользователя: Авторизованный Trainer");
                choice = await _trainerMenu.Execute(_context);
            }
        } while (choice != 0);
    }
}