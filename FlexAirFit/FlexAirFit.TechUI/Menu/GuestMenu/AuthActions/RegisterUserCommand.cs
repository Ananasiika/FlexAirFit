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
            Console.Write("Введите адрес электронной почты: ");
            email = Console.ReadLine();
            if (email is null || !email.Contains('@') || !email.Contains('.'))
            {
                isIncorrect = true;
                Console.WriteLine("Ошибка: Введенный адрес имеет некорректный формат");
            }
            else
            {
                isIncorrect = false;
            }
        } while (isIncorrect);
        
        do
        {
            Console.Write("Введите пароль: ");
            password = Console.ReadLine();
            if (password is null || password.Length < 3)
            {
                isIncorrect = true;
                Console.WriteLine("Ошибка: Пароль должен содержать 8 символов и более");
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
            Console.Write("Введите номер роли (1 - клиент, 2 - админ, 3 - тренер): ");
            numRole = Console.ReadLine();
            if (int.TryParse(numRole, out int parsedNumber))
            {
                if (parsedNumber < 1 || parsedNumber > 3)
                {
                    isIncorrect = true;
                    Console.WriteLine("Ошибка: Введенное значение не 1, 2 или 3");
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
                Console.WriteLine("Ошибка: Введено не число");
            }
        } while (isIncorrect);

        var userId = Guid.NewGuid();
        var user = new User(userId,
            role,
            email,
            password);
        
        try
        {
            await context.UserService.CreateUser(user, password, role);
            context.CurrentUser = user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОшибка: {ex.Message}\n");
            return;
        }

        if (role == UserRole.Client)
        {
            Console.WriteLine("Введите ваше имя:");
            string name = Console.ReadLine();

            Console.WriteLine("Выберите ваш пол (male, female):");
            string gender = Console.ReadLine();
            
            DateTime dateOfBirth;
            do
            {
                Console.WriteLine("Введите вашу дату рождения в формате YYYY-MM-DD:");
                if (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
                {
                    isIncorrect = true;
                    Console.WriteLine("Ошибка: Неверный формат даты рождения");
                }
                else
                {
                    isIncorrect = false;
                }
            } while (isIncorrect);

            Guid idMembership;
            do
            {
                Console.WriteLine("Введите ID абонемента:");
                if (!Guid.TryParse(Console.ReadLine(), out idMembership))
                {
                    Console.WriteLine("Ошибка: Неверный формат ID абонемента.");
                    isIncorrect = true;
                }
                else if (!context.MembershipService.CheckIfMembershipExists(idMembership).Result)
                {
                    isIncorrect = true;
                    Console.WriteLine("Ошибка: Абонемента с таким id не существует");
                }
                else
                {
                    isIncorrect = false;
                }
            } while (isIncorrect);

            Membership membership = context.MembershipService.GetMembershipById(idMembership).Result;
            Client client = new(userId, name, gender, dateOfBirth, idMembership, DateTime.Today.Add(membership.Duration), membership.Freezing, null);
            Bonus bonus = new(Guid.NewGuid(), userId, 0);
            try
            {
                await context.ClientService.CreateClient(client);
                await context.BonusService.CreateBonus(bonus);
                Console.WriteLine("Регистрация прошла успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}\n");
                return;
            }
        }
        else if (role == UserRole.Trainer)
        {
            Console.WriteLine("Введите ваше имя:");
            string name = Console.ReadLine();

            Console.WriteLine("Выберите ваш пол (male, female):");
            string gender = Console.ReadLine();
            
            Console.WriteLine("Введите вашу специализацию:");
            string specialization = Console.ReadLine();

            int experience;
            do
            {
                Console.WriteLine("Введите ваш опыт:");
                if (!int.TryParse(Console.ReadLine(), out experience))
                {
                    isIncorrect = true;
                    Console.WriteLine("Ошибка: Неверный формат количества лет опыта");
                }
                else
                {
                    isIncorrect = false;
                }
            } while (isIncorrect);
            
            int rating;
            do
            {
                Console.WriteLine("Введите ваш рейтинг (от 1 до 5):");
                if (!int.TryParse(Console.ReadLine(), out rating) || rating < 1 || rating > 5)
                {
                    isIncorrect = true;
                    Console.WriteLine("Ошибка: Неверный формат рейтинга");
                }
                else
                {
                    isIncorrect = false;
                }
            } while (isIncorrect);

            Trainer trainer = new(userId, name, gender, specialization, experience, rating);
            try
            {
                await context.TrainerService.CreateTrainer(trainer);
                Console.WriteLine("Регистрация прошла успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}\n");
                return;
            }
        }
        else
        {
            Console.WriteLine("Введите ваше имя:");
            string name = Console.ReadLine();
            
            DateTime dateOfBirth;
            do
            {
                Console.WriteLine("Введите вашу дату рождения в формате YYYY-MM-DD:");
                if (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
                {
                    isIncorrect = true;
                    Console.WriteLine("Ошибка: Неверный формат даты рождения");
                }
                else
                {
                    isIncorrect = false;
                }
            } while (isIncorrect);
            
            Console.WriteLine("Выберите ваш пол (male, female):");
            string gender = Console.ReadLine();

            Admin admin = new(userId, name, dateOfBirth.ToUniversalTime(), gender);
            try
            {
                await context.AdminService.CreateAdmin(admin);
                Console.WriteLine("Регистрация прошла успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}\n");
                return;
            }
        }

    }
}

