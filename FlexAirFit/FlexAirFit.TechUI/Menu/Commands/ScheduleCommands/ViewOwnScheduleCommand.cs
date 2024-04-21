using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Filters;
using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class ViewOwnScheduleCommand : Command
{
    public override string? Description()
    {
        return "Просмотр расписания своих тренировок";
    }

    public override async Task Execute(Context context)
    {
        FilterSchedule filter;
        if (context.CurrentUser.Role == UserRole.Client)
        {
            filter = new(null, null, null, context.CurrentUser.Id, null);
        }
        else
        {
            filter = new(null, null, null, null, context.CurrentUser.Id);
        }
        
        var schedules = await context.ScheduleService.GetScheduleByFilter(filter);
        
        if (schedules.Count == 0)
        {
            Console.WriteLine("Записей в расписании нет");
        }
        else
        {
            Console.WriteLine("Расписание:");

            foreach (var schedule in schedules)
            {
                Console.WriteLine($"ID: {schedule.Id}");
                Console.WriteLine($"ID тренировки: {schedule.IdWorkout}");
                Console.WriteLine($"Дата и время: {schedule.DateAndTime}");
        
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
        }
    }
}