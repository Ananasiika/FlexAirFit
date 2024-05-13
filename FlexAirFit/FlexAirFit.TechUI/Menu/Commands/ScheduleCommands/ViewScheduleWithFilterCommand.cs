using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.Core.Filters;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class ViewScheduleWithFilterCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewScheduleWithFilterCommand>();

    public override string? Description()
    {
        return "Просмотр расписания с фильтрацией";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewScheduleWithFilterCommand");

        Console.WriteLine("Введите название тренировки (или нажмите Enter, чтобы пропустить):");
        string nameWorkout = Console.ReadLine();
        _logger.Information("User entered workout name: {WorkoutName}", nameWorkout);

        DateTime? minDateAndTime = null;
        DateTime? maxDateAndTime = null;

        Console.WriteLine("Введите минимальную дату и время (или нажмите Enter, чтобы пропустить):");
        string minDateTimeInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(minDateTimeInput))
        {
            if (DateTime.TryParse(minDateTimeInput, out DateTime parsedMinDateTime))
            {
                minDateAndTime = parsedMinDateTime.ToUniversalTime();
                _logger.Information("User entered minimum date and time: {MinDateTime}", minDateAndTime);
            }
            else
            {
                _logger.Warning("Invalid date and time format entered for minimum date and time");
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
                _logger.Information("User entered maximum date and time: {MaxDateTime}", maxDateAndTime);
            }
            else
            {
                _logger.Warning("Invalid date and time format entered for maximum date and time");
                Console.WriteLine("Ошибка: Неверный формат даты и времени.");
                return;
            }
        }

        FilterSchedule filterSchedule = new(nameWorkout, minDateAndTime, maxDateAndTime, null, null, null);

        var schedules = await context.ScheduleService.GetScheduleByFilter(filterSchedule, 10, null);

        if (schedules.Count == 0)
        {
            _logger.Information("No schedule items found with the given filter");
            Console.WriteLine("Подходящих записей в расписании нет");
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
                    _logger.Information("User exited the command");
                    return;
                }
            }

            foreach (var schedule in schedules)

            {
                _logger.Information("Displaying schedule item: {@Schedule}", schedule);
                Console.WriteLine($"ID: {schedule.Id}");
                Console.WriteLine($"ID тренировки: {schedule.IdWorkout}");
                Console.WriteLine($"Название тренировки: {await context.WorkoutService.GetWorkoutNameById(schedule.IdWorkout)}");
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

            schedules = await context.ScheduleService.GetScheduleByFilter(filterSchedule, 10, 10 * page++);
        }
    }
}
