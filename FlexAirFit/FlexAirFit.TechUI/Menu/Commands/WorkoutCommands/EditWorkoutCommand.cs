using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.WorkoutCommands;

public class EditWorkoutCommand : Command
{
    public override string? Description()
    {
        return "Изменить существующую тренировку";
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
        
        Console.WriteLine("Введите новое название тренировки (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();

        Console.WriteLine("Введите новое описание тренировки (или нажмите Enter, чтобы пропустить):");
        string description = Console.ReadLine();

        Console.WriteLine("Введите новый ID тренера (или нажмите Enter, чтобы пропустить):");
        Guid idTrainer = Guid.Empty;
        string idTrainerInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(idTrainerInput))
        {
            if (!Guid.TryParse(idTrainerInput, out idTrainer))
            {
                Console.WriteLine("Ошибка: Неверный формат ID тренера.");
                return;
            }
        }
        
        Console.WriteLine("Введите новую продолжительность тренировки в формате (чч:мм:сс) (или нажмите Enter, чтобы пропустить):");
        TimeSpan duration = TimeSpan.Zero;
        string durationInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(durationInput))
        {
            if (!TimeSpan.TryParse(durationInput, out duration))
            {
                Console.WriteLine("Ошибка: Неверный формат продолжительности тренировки.");
                return;
            }
        }

        Console.WriteLine("Введите уровень тренировки (от 1 до 5) (или нажмите Enter, чтобы пропустить):");
        int level = 0;
        string levelInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(levelInput))
        {
            if (!int.TryParse(levelInput, out level))
            {
                Console.WriteLine("Ошибка: Неверный формат уровня тренировки.");
                return;
            }
        }

        Workout workout = await context.WorkoutService.GetWorkoutById(workoutId);
        workout.Description = description;
        workout.Name = name;
        workout.Level = (level == 0) ? workout.Level : level;
        workout.Duration = (duration == TimeSpan.Zero) ? workout.Duration : duration;
        workout.IdTrainer = (idTrainer == Guid.Empty) ? workout.IdTrainer : idTrainer;
        
        
        await context.WorkoutService.UpdateWorkout(workout);
        Console.WriteLine("Тренировка успешно обновлена.");
    }
}
