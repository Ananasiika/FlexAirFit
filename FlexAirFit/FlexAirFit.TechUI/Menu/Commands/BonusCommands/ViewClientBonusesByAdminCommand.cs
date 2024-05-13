using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.BonusCommands;

public class ViewClientBonusesByAdminCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewClientBonusesByAdminCommand>();

    public override string? Description()
    {
        return "Просмотр количества бонусов у клиента";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewClientBonusesByAdminCommand");

        Guid clientId;
        Console.Write("Введите id клиента: ");
        string clientIdInput = Console.ReadLine();
        _logger.Information($"Entered client id: {clientIdInput}");

        if (!Guid.TryParse(clientIdInput, out clientId))
        {
            _logger.Error("Invalid client id format");
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id клиента");
            return;
        }

        if (!context.ClientService.CheckIfClientExists(clientId).Result)
        {
            _logger.Error($"Client with id {clientId} not found");
            Console.WriteLine("Ошибка: Клиента с таким id не существует");
            return;
        }

        int bonuses = await context.BonusService.GetCountBonusByIdClient(clientId);
        Console.WriteLine($"Бонусов у клиента: {bonuses}");
    }
}