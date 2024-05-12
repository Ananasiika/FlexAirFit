using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Filters;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class ViewScheduleByClientCommand : Command
{
    public override string? Description()
    {
        return "Просмотр расписания";
    }

    public override async Task Execute(Context context)
    {
        FilterSchedule filter1 = new(null, null, null, null, context.CurrentUser.Id, null); 
        FilterSchedule filter2 = new(null, null, null, WorkoutType.GroupWorkout, null, null);
        
        var schedules = await context.ScheduleService.GetScheduleByFilter(filter1, 10, null);
        if (schedules.Count == 0)
        {
            Console.WriteLine("Персональных тренировок нет");
        }
        else
        {
            Console.WriteLine("Персональные тренировки:");
            int page = 1;
            while (schedules.Count != 0)
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
                        break;
                    }
                }
    
                foreach (var schedule in schedules)
                {
                    Console.WriteLine($"ID: {schedule.Id}");
                    Console.WriteLine($"ID тренировки: {schedule.IdWorkout}");
                    Console.WriteLine(
                        $"Название тренировки: {await context.WorkoutService.GetWorkoutNameById(schedule.IdWorkout)}");
                    Console.WriteLine($"Дата и время: {schedule.DateAndTime.ToLocalTime()}");
                    Console.WriteLine();
                }
                schedules = await context.ScheduleService.GetSchedules(10, 10 * page++);
            }
        }
        
        schedules = await context.ScheduleService.GetScheduleByFilter(filter2, 10, null);
        if (schedules.Count == 0)
        {
            Console.WriteLine("Групповых тренировок нет");
        }
        else
        {
            Console.WriteLine("Групповые тренировки:");
            int page = 1;
            while (schedules.Count != 0)
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
                        break;
                    }
                }
    
                foreach (var schedule in schedules)
                {
                    Console.WriteLine($"ID: {schedule.Id}");
                    Console.WriteLine($"ID тренировки: {schedule.IdWorkout}");
                    Console.WriteLine(
                        $"Название тренировки: {await context.WorkoutService.GetWorkoutNameById(schedule.IdWorkout)}");
                    Console.WriteLine($"Дата и время: {schedule.DateAndTime.ToLocalTime()}");
                    Console.WriteLine();
                }
                schedules = await context.ScheduleService.GetSchedules(10, 10 * page++);
            }
        }
    }
}