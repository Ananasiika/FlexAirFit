using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.WorkoutCommands;

public class ViewWorkoutCommand : Command
{
    public override string? Description()
    {
        return "Просмотр тренировок";
    }
    
    public override async Task Execute(Context context)
    {
        var workouts = await context.WorkoutService.GetWorkouts(10, null);
        if (workouts.Count == 0)
        {
            Console.WriteLine("Тренировок нет");
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
                    return;
                }
            }
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
            workouts = await context.WorkoutService.GetWorkouts(10, 10 * page++);
        }
    }
}