using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.BonusCommands;

public class EditClientBonusesCommand : Command
{
    public override string? Description()
    {
        return "Изменить количество бонусов клиента";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите id клиента: ");
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
        
        Console.WriteLine("Введите новое количество бонусов:");

        if (!int.TryParse(Console.ReadLine(), out int bonus))
        {
            Console.WriteLine("Ошибка: Неверный формат количства бонусов.");
            return;
        }

        await context.BonusService.UpdateCountBonusByIdClient(clientId, bonus);
        Console.WriteLine("Бонусы успешно обновлены.");
    }
}
