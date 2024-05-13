using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.FreezingCommands;

public class FreezeMembershipCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<FreezeMembershipCommand>();

    public override string? Description()
    {
        return "Заморозить абонемент";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing FreezeMembershipCommand");

        Console.WriteLine("Введите количество дней, на которое хотите заморозить абонемент:");
        string daysInput = Console.ReadLine();
        if (!int.TryParse(daysInput, out int days))
        {
            _logger.Warning("Invalid number of days format entered");
            Console.WriteLine("Ошибка: Неверный формат количества дней для заморозки.");
            return;
        }
        _logger.Information("Number of days to freeze: {Days}", days);

        if (days < 7)
        {
            _logger.Warning("Trying to freeze membership for less than 7 days");
            Console.WriteLine("Ошибка: Нельзя заморозить абонемент менее, чем на 7 дней.");
            return;
        }

        Console.WriteLine("Введите дату начала заморозки YYYY-MM-DD:");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateStartFreezing))
        {
            _logger.Warning("Invalid start freezing date format entered");
            Console.WriteLine("Ошибка: Неверный формат даты начала заморозки");
            return;
        }

        _logger.Information("Start freezing date entered: {StartFreezingDate}", dateStartFreezing);

        if (dateStartFreezing <= DateTime.Today)
        {
            _logger.Warning("Trying to set start freezing date earlier than today");
            Console.WriteLine("Ошибка: Дата начала заморозки не может быть ранее, чем сегодня.");
            return;
        }

        try
        {
            await context.ClientService.FreezeMembership(context.CurrentUser.Id, dateStartFreezing, days);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while freezing membership for client {ClientId}", context.CurrentUser.Id);
            Console.WriteLine("Ошибка: При заморозке абонемента произошла ошибка.");
        }
    }
}
