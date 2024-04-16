using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class AddMembershipToClientCommand : Command
{
    public override string? Description()
    {
        return "Добавить абонемент клиенту";
    }

    public override async Task Execute(Context context)
    {
        Console.Write("Введите id клиента: ");
        string clientIdInput = Console.ReadLine();
        if (!Guid.TryParse(clientIdInput, out Guid clientId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id клиента");
            return;
        }

        if (!context.ClientService.CheckIfClientExists(clientId).Result)
        {
            Console.WriteLine("Ошибка: Клиента с таким id не существует");
            return;
        }
        
        Console.WriteLine("Введите ID абонемента:");
        if (!Guid.TryParse(Console.ReadLine(), out Guid idMembership))
        {
            Console.WriteLine("Ошибка: Неверный формат ID абонемента.");
            return;
        }
        else if (!context.MembershipService.CheckIfMembershipExists(idMembership).Result)
        {
            Console.WriteLine("Ошибка: Абонемента с таким id не существует");
            return;
        }

        try
        {
            await context.ClientService.AddMembership(clientId, idMembership);
        }
        catch (Exception e)
        {
            Console.WriteLine("При добавлении абонемента произошла ошибка", e);
            return;
        }
        Console.WriteLine("Абонемент добавлен клиенту");
    }
}