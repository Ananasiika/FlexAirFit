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
    private readonly Menu _authorizedMenu = menus[1];
    private readonly Menu _adminMenu = menus[2];

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
            else
            {
                Console.WriteLine($"Статус пользователя: Авторизованный ({_context.CurrentUser.Role})");
                choice = await _authorizedMenu.Execute(_context);
            }
        } while (choice != 0);
    }
}