using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Enums;
using Serilog;

namespace FlexAirFit.TechUI.Commands.WorkoutCommands;

public class AddWorkoutCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<AddWorkoutCommand>();

    public override string? Description()
    {
        return "Добавить новую тренировку";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing AddWorkoutCommand");

        Console.WriteLine("Введите название тренировки:");
        string name = Console.ReadLine();
        _logger.Information("User entered workout name: {WorkoutName}", name);

        Console.WriteLine("Введите описание тренировки:");
        string description = Console.ReadLine();
        _logger.Information("User entered workout description: {WorkoutDescription}", description);

        Guid idTrainer;
        if (context.CurrentUser.Role != UserRole.Trainer)
        {
            Console.WriteLine("Введите ID тренера:");
            string trainerIdInput = Console.ReadLine();
            if (!Guid.TryParse(trainerIdInput, out idTrainer))
            {
                _logger.Error("Invalid trainer ID format entered by user: {TrainerIdInput}", trainerIdInput);
                Console.WriteLine("Ошибка: Неверный формат ID тренера.");
                return;
            }

            if (!context.TrainerService.CheckIfTrainerExists(idTrainer).Result)
            {
                _logger.Error("Trainer with ID {TrainerId} does not exist", idTrainer);
                Console.WriteLine("Ошибка: Тренера с таким id не существует");
                return;
            }
            _logger.Information("User entered trainer ID: {TrainerId}", idTrainer);
        }
        else
        {
            idTrainer = context.CurrentUser.Id;
            _logger.Information("Using current user as trainer with ID: {TrainerId}", idTrainer);
        }

        Console.WriteLine("Введите продолжительность тренировки в формате (чч:мм:сс):");
        string durationInput = Console.ReadLine();
        TimeSpan duration;
        if (!TimeSpan.TryParse(durationInput, out duration))
        {
            _logger.Error("Invalid workout duration format entered by user: {DurationInput}", durationInput);
            Console.WriteLine("Ошибка: Неверный формат продолжительности тренировки.");
            return;
        }
        _logger.Information("User entered workout duration: {Duration}", duration);

        Console.WriteLine("Введите уровень тренировки (от 1 до 5):");
        string levelInput = Console.ReadLine();
        int level;
        if (!int.TryParse(levelInput, out level))
        {
            _logger.Error("Invalid workout level format entered by user: {LevelInput}", levelInput);
            Console.WriteLine("Ошибка: Неверный формат уровня тренировки.");
            return;
        }
        _logger.Information("User entered workout level: {WorkoutLevel}", level);

        Workout workout = new(Guid.NewGuid(), name, description, idTrainer, duration, level);

        try
        {
            await context.WorkoutService.CreateWorkout(workout);
            Console.WriteLine("Тренировка успешно добавлена.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating workout");
            Console.WriteLine("Ошибка при создании тренировки.");
            return;
        }
    }
}
