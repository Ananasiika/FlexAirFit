using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.GuestMenu.AuthActions;

public class SignInUserCommand : Command
{
    public override string? Description()
    {
        return "Авторизоваться";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("\n========== Авторизация ==========");

        Console.Write("Введите адрес электронной почты: ");
        var email = Console.ReadLine();

        Console.Write("Введите пароль: ");
        var password = Console.ReadLine();

        if (email is null && password is null)
        {
            Console.WriteLine("Ошибка: Неверный ввод\n");
            return;
        }

        try
        {
            var user = await context.UserService.SignInUser(email!, password!);
            context.CurrentUser = user;
            Console.WriteLine("Авторизация прошла успешно");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОшибка: {ex.Message}\n");
        }
    }
}
