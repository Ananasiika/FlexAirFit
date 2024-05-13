using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.FreezingCommands;

public class FreezeMembershipByAdminCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<FreezeMembershipByAdminCommand>();

    public override string? Description()
    {
        return "Заморозить абонемент у клиента";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing FreezeMembershipByAdminCommand");

        Guid clientId;
        Console.Write("Введите id клиента: ");
        string clientIdInput = Console.ReadLine();
        if (!Guid.TryParse(clientIdInput, out clientId))
        {
            _logger.Warning("Invalid client id format entered");
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id клиента");
            return;
        }

        _logger.Information("Client id entered: {ClientId}", clientId);

        if (!context.ClientService.CheckIfClientExists(clientId).Result)
        {
            _logger.Warning("Client with id {ClientId} does not exist", clientId);
            Console.WriteLine("Ошибка: Клиента с таким id не существует");
            return;
        }

        Console.WriteLine("Введите количество дней, на которое хотите заморозить абонемент:");
        string daysInput = Console.ReadLine();
        if (!int.TryParse(daysInput, out int days))
        {
            _logger.Warning("Invalid number of days format entered");
            Console.WriteLine("Ошибка: Неверный формат количества дней для заморозки.");
            return;
        }

        _logger.Information("Number of days to freeze: {Days}", days);

        DateTime dateStartFreezing;
        Console.WriteLine("Введите дату начала заморозки YYYY-MM-DD:");
        if (!DateTime.TryParse(Console.ReadLine(), out dateStartFreezing))
        {
            _logger.Warning("Invalid start freezing date format entered");
            Console.WriteLine("Ошибка: Неверный формат даты начала заморозки");
            return;
        }

        _logger.Information("Start freezing date entered: {StartFreezingDate}", dateStartFreezing);

        try
        {
            await context.ClientService.FreezeMembership(clientId, dateStartFreezing, days);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while freezing membership for client {ClientId}", clientId);
            throw;
        }
    }
}
