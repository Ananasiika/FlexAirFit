using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.WorkoutCommands;

public class EditWorkoutCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<EditWorkoutCommand>();

    public override string? Description()
    {
        return "Изменить существующую тренировку";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing EditWorkoutCommand for user {UserId}", context.CurrentUser.Id);

        Console.WriteLine("Введите id тренировки: ");
        string workoutIdInput = Console.ReadLine();
        _logger.Information("User entered workout ID: {WorkoutIdInput}", workoutIdInput);

        if (!Guid.TryParse(workoutIdInput, out Guid workoutId))
        {
            _logger.Error("Invalid workout ID format entered by user: {WorkoutIdInput}", workoutIdInput);
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id тренировки");
            return;
        }

        if (!context.WorkoutService.CheckIfWorkoutExists(workoutId).Result)
        {
            _logger.Error("Workout with ID {WorkoutId} does not exist", workoutId);
            Console.WriteLine("Ошибка: Тренировки с таким id не существует");
            return;
        }

        Console.WriteLine("Введите новое название тренировки (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();
        _logger.Information("User entered new workout name: {NewName}", name);

        Console.WriteLine("Введите новое описание тренировки (или нажмите Enter, чтобы пропустить):");
        string description = Console.ReadLine();
        _logger.Information("User entered new workout description: {NewDescription}", description);

        Console.WriteLine("Введите новый ID тренера (или нажмите Enter, чтобы пропустить):");
        Guid idTrainer = Guid.Empty;
        string idTrainerInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(idTrainerInput))
        {
            if (!Guid.TryParse(idTrainerInput, out idTrainer))
            {
                _logger.Error("Invalid trainer ID format entered by user: {TrainerIdInput}", idTrainerInput);
                Console.WriteLine("Ошибка: Неверный формат ID тренера.");
                return;
            }
            _logger.Information("User entered new trainer ID: {NewTrainerId}", idTrainer);
        }
        else
        {
            _logger.Information("User skipped entering new trainer ID");
        }

        Console.WriteLine("Введите новую продолжительность тренировки в формате (чч:мм:сс) (или нажмите Enter, чтобы пропустить):");
        TimeSpan duration = TimeSpan.Zero;
        string durationInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(durationInput))
        {
            if (!TimeSpan.TryParse(durationInput, out duration))
            {
                _logger.Error("Invalid workout duration format entered by user: {DurationInput}", durationInput);
                Console.WriteLine("Ошибка: Неверный формат продолжительности тренировки.");
                return;
            }
            _logger.Information("User entered new workout duration: {NewDuration}", duration);
        }
        else
        {
            _logger.Information("User skipped entering new workout duration");
        }

        Console.WriteLine("Введите уровень тренировки (от 1 до 5) (или нажмите Enter, чтобы пропустить):");
        int level = 0;
        string levelInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(levelInput))
        {

            if (!int.TryParse(levelInput, out level))
            {
                _logger.Error("Invalid workout level format entered by user: {LevelInput}", levelInput);
                Console.WriteLine("Ошибка: Неверный формат уровня тренировки.");
                return;
            }
            _logger.Information("User entered new workout level: {NewLevel}", level);
        }
        else
        {
            _logger.Information("User skipped entering new workout level");
        }

        Workout workout = await context.WorkoutService.GetWorkoutById(workoutId);

        workout.Description = string.IsNullOrEmpty(description) ? workout.Description : description;
        workout.Name = string.IsNullOrEmpty(name) ? workout.Name : name;
        workout.Level = level == 0 ? workout.Level : level;
        workout.Duration = duration == TimeSpan.Zero ? workout.Duration : duration;
        workout.IdTrainer = idTrainer == Guid.Empty ? workout.IdTrainer : idTrainer;

        try
        {
            await context.WorkoutService.UpdateWorkout(workout);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating workout with ID {WorkoutId}", workoutId);
            Console.WriteLine("При обновлении произошла ошибка", ex);
            return;
        }

        Console.WriteLine("Тренировка успешно обновлена.");
    }
}
