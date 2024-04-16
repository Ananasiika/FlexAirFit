using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class DeleteRecordFromScheduleCommand : Command
{
    public override string? Description()
    {
        return "Удалить существующую запись в расписании";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите id записи: ");
        string scheduleIdInput = Console.ReadLine();
        if (!Guid.TryParse(scheduleIdInput, out Guid scheduleId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id записи");
            return;
        }

        if (!context.ScheduleService.CheckIfScheduleExists(scheduleId).Result)
        {
            Console.WriteLine("Ошибка: Записи с таким id не существует");
            return;
        }

        try
        {
            await context.ScheduleService.DeleteSchedule(scheduleId);
        }
        catch (Exception e)
        {
            Console.WriteLine("При удалении произошла ошибка", e);
            return;
        }
        Console.WriteLine("Запись успешно удалена");
    }
}
