using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class AddMembershipToClientCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<AddMembershipToClientCommand>();
    public override string? Description()
    {
        return "Добавить абонемент клиенту";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing AddMembershipToClientCommand");

        Console.Write("Введите id клиента: ");
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

        Console.WriteLine("Введите ID абонемента:");
        string membershipIdInput = Console.ReadLine();
        _logger.Information($"Entered membership id: {membershipIdInput}");

        if (!Guid.TryParse(membershipIdInput, out Guid idMembership))
        {
            _logger.Error("Invalid membership id format");
            Console.WriteLine("Ошибка: Неверный формат ID абонемента.");
            return;
        }
        else if (!context.MembershipService.CheckIfMembershipExists(idMembership).Result)
        {
            _logger.Error($"Membership with id {idMembership} not found");
            Console.WriteLine("Ошибка: Абонемента с таким id не существует");
            return;
        }

        try
        {
            await context.ClientService.AddMembership(clientId, idMembership);
        }
        catch (Exception e)
        {
            _logger.Error($"Error adding membership {idMembership} to client {clientId}: {e.Message}");
            Console.WriteLine("При добавлении абонемента произошла ошибка", e);
            return;
        }
        Console.WriteLine("Абонемент добавлен клиенту");
    }
}
