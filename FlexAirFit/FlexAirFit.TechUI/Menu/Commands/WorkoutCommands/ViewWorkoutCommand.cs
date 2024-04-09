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
        var workouts = await context.WorkoutService.GetWorkouts(null, null);
        if (workouts.Count == 0)
        {
            Console.WriteLine("Тренировок нет");
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