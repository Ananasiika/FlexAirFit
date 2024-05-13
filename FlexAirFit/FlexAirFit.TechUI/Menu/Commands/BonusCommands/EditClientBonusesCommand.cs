using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.BonusCommands;

public class EditClientBonusesCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<EditClientBonusesCommand>();
    public override string? Description()
    {
        return "Изменить количество бонусов клиента";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing EditClientBonusesCommand");
        
        Console.WriteLine("Введите id клиента: ");
        string clientIdInput = Console.ReadLine();
        _logger.Information($"Entered client id: {clientIdInput}");
        
        if (!Guid.TryParse(clientIdInput, out Guid clientId))
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
        
        Console.WriteLine("Введите новое количество бонусов:");
        if (!int.TryParse(Console.ReadLine(), out int bonus))
        {
            _logger.Error("Invalid bonus format");
            Console.WriteLine("Ошибка: Неверный формат количства бонусов.");
            return;
        }

        await context.BonusService.UpdateCountBonusByIdClient(clientId, bonus);
        Console.WriteLine("Бонусы успешно обновлены.");
    }
}
