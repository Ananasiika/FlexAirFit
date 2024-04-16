using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.WorkoutCommands;

public class DeleteWorkoutCommand : Command
{
    public override string? Description()
    {
        return "Удалить существующую тренировку";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите id тренировки: ");
        string workoutIdInput = Console.ReadLine();
        if (!Guid.TryParse(workoutIdInput, out Guid workoutId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id тренировки");
            return;
        }

        if (!context.WorkoutService.CheckIfWorkoutExists(workoutId).Result)
        {
            Console.WriteLine("Ошибка: Тренировки с таким id не существует");
            return;
        }

        try
        {
            await context.WorkoutService.DeleteWorkout(workoutId);
        }
        catch (Exception e)
        {
            Console.WriteLine("При удалении произошла ошибка", e);
            return;
        }
        Console.WriteLine("Тренировка успешно удалена");
    }
}