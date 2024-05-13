using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.WorkoutCommands;

public class DeleteWorkoutCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<DeleteWorkoutCommand>();

    public override string? Description()
    {
        return "Удалить существующую тренировку";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing DeleteWorkoutCommand for user {UserId}", context.CurrentUser.Id);

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

        try
        {
            await context.WorkoutService.DeleteWorkout(workoutId);
            Console.WriteLine("Тренировка успешно удалена");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error deleting workout with ID {WorkoutId}", workoutId);
            Console.WriteLine("При удалении произошла ошибка", ex);
            return;
        }
    }
}