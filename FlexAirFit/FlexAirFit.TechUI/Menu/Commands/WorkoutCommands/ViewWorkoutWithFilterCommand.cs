using FlexAirFit.Core.Filters;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.WorkoutCommands;

public class ViewWorkoutWithFilterCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewWorkoutWithFilterCommand>();

    public override string? Description()
    {
        return "Просмотр тренировок с фильтрацией";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewWorkoutWithFilterCommand for user {UserId}", context.CurrentUser.Id);

        Console.WriteLine("Введите название тренировки (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();
        _logger.Information("User entered workout name: {WorkoutName}", name);

        Console.WriteLine("Введите имя тренера (или нажмите Enter, чтобы пропустить):");
        string nameTrainer = Console.ReadLine();
        _logger.Information("User entered trainer name: {TrainerName}", nameTrainer);

        TimeSpan? minDuration = null;
        TimeSpan? maxDuration = null;

        Console.WriteLine("Введите минимальную продолжительность тренировки в формате (чч:мм:сс) (или нажмите Enter, чтобы пропустить):");
        string minDurationInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(minDurationInput))
        {
            if (TimeSpan.TryParse(minDurationInput, out TimeSpan parsedMinDuration))
            {
                minDuration = parsedMinDuration;
                _logger.Information("User entered minimum workout duration: {MinDuration}", parsedMinDuration);
            }
            else
            {
                _logger.Error("Invalid minimum workout duration format entered by user: {MinDurationInput}", minDurationInput);
                Console.WriteLine("Ошибка: Неверный формат минимальной продолжительности тренировки.");
                return;
            }
        }
        else
        {
            _logger.Information("User skipped entering minimum workout duration");
        }

        Console.WriteLine("Введите максимальную продолжительность тренировки в формате (чч:мм:сс) (или нажмите Enter, чтобы пропустить):");
        string maxDurationInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(maxDurationInput))
        {
            if (TimeSpan.TryParse(maxDurationInput, out TimeSpan parsedMaxDuration))
            {
                maxDuration = parsedMaxDuration;
                _logger.Information("User entered maximum workout duration: {MaxDuration}", parsedMaxDuration);
            }
            else
            {
                _logger.Error("Invalid maximum workout duration format entered by user: {MaxDurationInput}", maxDurationInput);
                Console.WriteLine("Ошибка: Неверный формат максимальной продолжительности тренировки.");
                return;
            }
        }
        else
        {
            _logger.Information("User skipped entering maximum workout duration");
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
                _logger.Information("User entered minimum workout level: {MinLevel}", parsedMinLevel);
            }
            else
            {

                _logger.Error("Invalid minimum workout level format entered by user: {MinLevelInput}", minLevelInput);
                Console.WriteLine("Ошибка: Неверный формат минимального уровня тренировки.");
                return;
            }
        }
        else
        {
            _logger.Information("User skipped entering minimum workout level");
        }

        Console.WriteLine("Введите максимальный уровень тренировки (или нажмите Enter, чтобы пропустить):");
        string maxLevelInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(maxLevelInput))
        {
            if (int.TryParse(maxLevelInput, out int parsedMaxLevel))
            {
                maxLevel = parsedMaxLevel;
                _logger.Information("User entered maximum workout level: {MaxLevel}", parsedMaxLevel);
            }
            else
            {
                _logger.Error("Invalid maximum workout level format entered by user: {MaxLevelInput}", maxLevelInput);
                Console.WriteLine("Ошибка: Неверный формат максимального уровня тренировки.");
                return;
            }
        }
        else
        {
            _logger.Information("User skipped entering maximum workout level");
        }

        FilterWorkout filter = new(name, nameTrainer, minDuration, maxDuration, minLevel, maxLevel);
        var workouts = await context.WorkoutService.GetWorkoutByFilter(filter, 10, null);

        if (workouts.Count == 0)
        {
            _logger.Information("No workouts found for the given filter");
            Console.WriteLine("Подходящих тренировок нет");
            return;
        }

        Console.WriteLine("Список тренировок:");
        int page = 1;
        while (workouts.Count != 0)
        {
            if (page != 1)
            {
                Console.WriteLine("Введите 0, чтобы закончить или Enter, чтобы продолжить:");
                string nextInput = Console.ReadLine();
                if (!int.TryParse(nextInput, out int next))
                {
                    next = 1;
                }

                if (next == 0)
                {
                    _logger.Information("User exited workout listing");
                    return;
                }
            }

            foreach (var workout in workouts)
            {
                _logger.Information("Displaying workout: {@Workout}", workout);

                Console.WriteLine($"ID: {workout.Id}");
                Console.WriteLine($"Название: {workout.Name}");
                Console.WriteLine($"Описание: {workout.Description}");
                Console.WriteLine($"ID тренера: {workout.IdTrainer}");
                Console.WriteLine($"Продолжительность: {workout.Duration}");
                Console.WriteLine($"Уровень: {workout.Level}");
                Console.WriteLine();
            }

            workouts = await context.WorkoutService.GetWorkoutByFilter(filter, 10, 10 * page++);
        }
    }
}
