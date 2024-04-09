using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.BonusCommands;

public class ViewClientBonusesByAdminCommand : Command
{
    public override string? Description()
    {
        return "Просмотр количества бонусов у клиента";
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
        
        int bonuses = await context.BonusService.GetCountBonusByIdClient(clientId);
        Console.WriteLine($"Бонусов у клиента: {bonuses}");
    }
}