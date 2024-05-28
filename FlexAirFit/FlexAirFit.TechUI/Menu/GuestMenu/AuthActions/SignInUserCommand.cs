using FlexAirFit.Core.Enums;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;
using Serilog.Context;

namespace FlexAirFit.TechUI.GuestMenu.AuthActions;

public class SignInUserCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<SignInUserCommand>();
    public override string? Description()
    {
        return "Авторизоваться";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("\n========== Авторизация ==========");

        Console.Write("Введите адрес электронной почты: ");
        var email = Console.ReadLine();
        _logger.Information($"Entered email: {email}");

        Console.Write("Введите пароль: ");
        var password = Console.ReadLine();
        _logger.Information($"Entered password");

        if (email is null && password is null)
        {
            _logger.Error("Invalid input");
            Console.WriteLine("Ошибка: Неверный ввод\n");
            return;
        }

        try
        {
            var user = await context.UserService.SignInUser(email!, password!);
            context.CurrentUser = user;
            LogContext.PushProperty("UserRole", context.CurrentUser.Role.ToString());
            LogContext.PushProperty("UserId", context.CurrentUser.Id);
            Console.WriteLine("Авторизация прошла успешно");
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            Console.WriteLine($"\nОшибка: {ex.Message}\n");
        }
    }
}
