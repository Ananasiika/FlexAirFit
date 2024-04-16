using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class ViewScheduleWithFilterCommand : Command
{
    public override string? Description()
    {
        return "Просмотр расписания с фильтрацией";
    }
    
    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите название тренировки (или нажмите Enter, чтобы пропустить):");
        string nameWorkout = Console.ReadLine();
        
        DateTime? minDateAndTime = null;
        DateTime? maxDateAndTime = null;

        Console.WriteLine("Введите минимальную дату и время (или нажмите Enter, чтобы пропустить):");
        string minDateTimeInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(minDateTimeInput))
        {
            if (DateTime.TryParse(minDateTimeInput, out DateTime parsedMinDateTime))
            {
                minDateAndTime = parsedMinDateTime.ToUniversalTime();
            }
            else
            {
                Console.WriteLine("Ошибка: Неверный формат даты и времени.");
                return;
            }
        }

        Console.WriteLine("Введите максимальную дату и время (или нажмите Enter, чтобы пропустить):");
        string maxDateTimeInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(maxDateTimeInput))
        {
            if (DateTime.TryParse(maxDateTimeInput, out DateTime parsedMaxDateTime))
            {
                maxDateAndTime = parsedMaxDateTime.ToUniversalTime();
            }
            else
            {
                Console.WriteLine("Ошибка: Неверный формат даты и времени.");
                return;
            }
        }
        FilterSchedule filterSchedule = new(nameWorkout, minDateAndTime, maxDateAndTime, null, null);

        var schedules = await context.ScheduleService.GetScheduleByFilter(filterSchedule);
        
        if (schedules.Count == 0)
        {
            Console.WriteLine("Подходящих записей в расписании нет");
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