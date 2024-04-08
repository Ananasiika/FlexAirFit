using FlexAirFit.Core;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.GuestMenu.AuthActions;

public class RegisterUserCommand : Command
{
    public override string? Description()
    {
        return "Зарегистрироваться";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("\n========== Регистрация ==========");

        string password, passwordVerify, email, numRole;
        UserRole role = UserRole.Admin;
        bool isIncorrect;

        do
        {
            Console.Write("Введите пароль: ");
            password = Console.ReadLine();
            if (password is null || password.Length < 3)
            {
                isIncorrect = true;
                Console.WriteLine("[!] Пароль должен содержать 8 символов и более");
            }
            else
            {
                isIncorrect = false;
            }
        } while (isIncorrect);

        do
        {
            Console.Write("-> Подтвердите пароль: ");
            passwordVerify = Console.ReadLine();
        } while (password != passwordVerify);

        do
        {
            Console.Write("Введите адрес электронной почты: ");
            email = Console.ReadLine();
            if (email is null || !email.Contains('@') || !email.Contains('.'))
            {
                isIncorrect = true;
                Console.WriteLine("[!] Введенный адрес имеет некорректный формат");
            }
            else
            {
                isIncorrect = false;
            }
        } while (isIncorrect);
        
        do
        {
            Console.Write("Введите номер роли (1 - клиент, 2 - админ, 3 - тренер): ");
            numRole = Console.ReadLine();
            if (int.TryParse(numRole, out int parsedNumber))
            {
                if (parsedNumber < 1 || parsedNumber > 3)
                {
                    isIncorrect = true;
                    Console.WriteLine("[!] Введенное значение не 1, 2 или 3");
                }
                else
                {
                    isIncorrect = false;
                    role = (UserRole)(parsedNumber - 1);
                }
            }
            else
            {
                isIncorrect = true;
                Console.WriteLine("[!] Введено не число");
            }
        } while (isIncorrect);

        bool makeUser = context.CurrentUser is not null;

        try
        {
            var userId = Guid.NewGuid();
            var user = new User(userId,
                                role,
                                email,
                                password);
            await context.UserService.CreateUser(user, password, role);
            context.CurrentUser = user;
            Console.WriteLine("Регистрация прошла успешно");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
            return;
        }
    }
}

