using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.FreezingCommands;

public class FreezeMembershipByAdminCommand : Command
{
    public override string? Description()
    {
        return "Заморозить абонемент у клиента";
    }

    public override async Task Execute(Context context)
    {
        Guid clientId;
        Console.Write("Введите id клиента: ");
        string clientIdInput = Console.ReadLine();
        if (!Guid.TryParse(clientIdInput, out clientId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id клиента");
            return;
        }

        if (!context.ClientService.CheckIfClientExists(clientId).Result)
        {
            Console.WriteLine("Ошибка: Клиента с таким id не существует");
            return;
        }
        
        Console.WriteLine("Введите количество дней, на которое хотите заморозить абонемент:");
        string daysInput = Console.ReadLine();
        if (!int.TryParse(daysInput, out int days))
        {
            Console.WriteLine("Ошибка: Неверный формат количества дней для заморозки.");
            return;
        }
        DateTime dateStartFreezing;
        Console.WriteLine("Введите дату начала заморозки YYYY-MM-DD:");
        if (!DateTime.TryParse(Console.ReadLine(), out dateStartFreezing))
        {
            Console.WriteLine("Ошибка: Неверный формат даты начала заморозки");
            return;
        }

        await context.ClientService.FreezeMembership(clientId, dateStartFreezing, days);
    }
}