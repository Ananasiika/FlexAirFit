using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class DeleteRecordFromScheduleCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<DeleteRecordFromScheduleCommand>();

    public override string? Description()
    {
        return "Удалить существующую запись в расписании";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing DeleteRecordFromScheduleCommand");

        Console.WriteLine("Введите id записи: ");
        string scheduleIdInput = Console.ReadLine();
        if (!Guid.TryParse(scheduleIdInput, out Guid scheduleId))
        {
            _logger.Warning("Invalid schedule id format entered: {ScheduleIdInput}", scheduleIdInput);
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id записи");
            return;
        }

        _logger.Information("Schedule id entered: {ScheduleId}", scheduleId);

        if (!context.ScheduleService.CheckIfScheduleExists(scheduleId).Result)
        {
            _logger.Warning("Schedule with id {ScheduleId} does not exist", scheduleId);
            Console.WriteLine("Ошибка: Записи с таким id не существует");
            return;
        }

        try
        {
            await context.ScheduleService.DeleteSchedule(scheduleId);
            Console.WriteLine("Запись успешно удалена");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while deleting schedule with id {ScheduleId}", scheduleId);
            Console.WriteLine("При удалении произошла ошибка", ex);
            return;
        }
    }
}