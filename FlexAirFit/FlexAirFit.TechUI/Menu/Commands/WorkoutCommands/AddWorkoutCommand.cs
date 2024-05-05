using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.TechUI.Commands.WorkoutCommands;

public class AddWorkoutCommand : Command
{
    public override string? Description()
    {
        return "Добавить новую тренировку";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите название тренировки:");
        string name = Console.ReadLine();

        Console.WriteLine("Введите описание тренировки:");
        string description = Console.ReadLine();

        Guid idTrainer;
        if (context.CurrentUser.Role != UserRole.Trainer)
        {
            Console.WriteLine("Введите ID тренера:");
            if (!Guid.TryParse(Console.ReadLine(), out idTrainer))
            {
                Console.WriteLine("Ошибка: Неверный формат ID тренера.");
                return;
            }
            if (!context.TrainerService.CheckIfTrainerExists(idTrainer).Result)
            {
                Console.WriteLine("Ошибка: Тренера с таким id не существует");
                return;
            }
        }
        else
        {
            idTrainer = context.CurrentUser.Id;
        }
        
        Console.WriteLine("Введите продолжительность тренировки в формате (чч:мм:сс):");
        TimeSpan duration;
        if (!TimeSpan.TryParse(Console.ReadLine(), out duration))
        {
            Console.WriteLine("Ошибка: Неверный формат продолжительности тренировки.");
            return;
        }

        Console.WriteLine("Введите уровень тренировки (от 1 до 5):");
        int level;
        if (!int.TryParse(Console.ReadLine(), out level))
        {
            Console.WriteLine("Ошибка: Неверный формат уровня тренировки.");
            return;
        }

        Workout workout = new(Guid.NewGuid(), name, description, idTrainer, duration, level);
        
        await context.WorkoutService.CreateWorkout(workout);
        Console.WriteLine("Тренировка успешно добавлена.");
    }
}