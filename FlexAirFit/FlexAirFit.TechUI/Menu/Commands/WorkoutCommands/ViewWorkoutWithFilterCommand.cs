using FlexAirFit.Core.Filters;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.WorkoutCommands;

public class ViewWorkoutWithFilterCommand : Command
{
    public override string? Description()
    {
        return "Просмотр тренировок с фильтрацией";
    }
    
    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите название тренировки (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();

        Console.WriteLine("Введите имя тренера (или нажмите Enter, чтобы пропустить):");
        string nameTrainer = Console.ReadLine();

        TimeSpan? minDuration = null;
        TimeSpan? maxDuration = null;

        Console.WriteLine("Введите минимальную продолжительность тренировки в формате (чч:мм:сс) (или нажмите Enter, чтобы пропустить):");
        string minDurationInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(minDurationInput))
        {
            if (TimeSpan.TryParse(minDurationInput, out TimeSpan parsedMinDuration))
            {
                minDuration = parsedMinDuration;
            }
            else
            {
                Console.WriteLine("Ошибка: Неверный формат минимальной продолжительности тренировки.");
                return;
            }
        }

        Console.WriteLine("Введите максимальную продолжительность тренировки в формате (чч:мм:сс) (или нажмите Enter, чтобы пропустить):");
        string maxDurationInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(maxDurationInput))
        {
            if (TimeSpan.TryParse(maxDurationInput, out TimeSpan parsedMaxDuration))
            {
                maxDuration = parsedMaxDuration;
            }
            else
            {
                Console.WriteLine("Ошибка: Неверный формат максимальной продолжительности тренировки.");
                return;
            }
        }

        int? minLevel = null;
        int? maxLevel = null;

        Console.WriteLine("Введите минимальный уровень тренировки (или нажмите Enter, чтобы пропустить):");
        string minLevelInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(minLevelInput))
        {
            if (int.TryParse(minLevelInput, out int parsedMinLevel))
            {
                minLevel = parsedMinLevel;
            }
            else
            {
                Console.WriteLine("Ошибка: Неверный формат минимального уровня тренировки.");
                return;
            }
        }

        Console.WriteLine("Введите максимальный уровень тренировки (или нажмите Enter, чтобы пропустить):");
        string maxLevelInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(maxLevelInput))
        {
            if (int.TryParse(maxLevelInput, out int parsedMaxLevel))
            {
                maxLevel = parsedMaxLevel;
            }
            else
            {
                Console.WriteLine("Ошибка: Неверный формат максимального уровня тренировки.");
                return;
            }
        }

        FilterWorkout filter = new(name, nameTrainer, minDuration, maxDuration, minLevel, maxLevel);

        var workouts = await context.WorkoutService.GetWorkoutByFilter(filter);
        if (workouts.Count == 0)
        {
            Console.WriteLine("Подходящих тренировок нет");
        }
        else
        {
            Console.WriteLine("Список тренировок:");

            foreach (var workout in workouts)
            {
                Console.WriteLine($"ID: {workout.Id}");
                Console.WriteLine($"Название: {workout.Name}");
                Console.WriteLine($"Описание: {workout.Description}");
                Console.WriteLine($"ID тренера: {workout.IdTrainer}");
                Console.WriteLine($"Продолжительность: {workout.Duration}");
                Console.WriteLine($"Уровень: {workout.Level}");
                Console.WriteLine(); 
            }

        }
    }
}