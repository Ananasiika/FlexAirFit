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
        var schedules = await context.ScheduleService.GetSchedules(null, null);
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