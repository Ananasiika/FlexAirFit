using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Filters;
using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class ViewOwnScheduleCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewOwnScheduleCommand>();

    public override string? Description()
    {
        return "Просмотр расписания своих тренировок";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewOwnScheduleCommand");

        FilterSchedule filter;
        if (context.CurrentUser.Role == UserRole.Client)
        {
            filter = new(null, null, null, null, context.CurrentUser.Id, null);
            _logger.Information("Filter created for client with id {ClientId}", context.CurrentUser.Id);
        }
        else
        {
            filter = new(null, null, null, null, null, context.CurrentUser.Id);
            _logger.Information("Filter created for trainer with id {TrainerId}", context.CurrentUser.Id);
        }

        var schedules = await context.ScheduleService.GetScheduleByFilter(filter, 10, null);

        if (schedules.Count == 0)
        {
            _logger.Information("No schedule items found");
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

            schedules = await context.ScheduleService.GetScheduleByFilter(filter, 10, 10 * page++);
        }
    }
}
