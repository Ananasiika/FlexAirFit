using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Filters;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class ViewScheduleCommand : Command
{
    public override string? Description()
    {
        return "Просмотр расписания";
    }
    
    public override async Task Execute(Context context)
    {
        var schedules = await context.ScheduleService.GetSchedules(10, null);
        if (schedules.Count == 0)
        {
            Console.WriteLine("Записей в расписании нет");
            return;
        }

        Console.WriteLine("Расписание:");
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
                    return;
                }
            }

            foreach (var schedule in schedules)
            {
                Console.WriteLine($"ID: {schedule.Id}");
                Console.WriteLine($"ID тренировки: {schedule.IdWorkout}");
                Console.WriteLine(
                    $"Название тренировки: {await context.WorkoutService.GetWorkoutNameById(schedule.IdWorkout)}");
                Console.WriteLine($"Дата и время: {schedule.DateAndTime.ToLocalTime()}");

                if (schedule.IdClient.HasValue && schedule.IdClient.Value != Guid.Empty)
                {
                    Console.WriteLine($"ID клиента: {schedule.IdClient}");
                }
                else
                {
                    Console.WriteLine("Групповая тренировка");
                }

                Console.WriteLine();
            }

            schedules = await context.ScheduleService.GetSchedules(10, 10 * page++);
        }
    }
}