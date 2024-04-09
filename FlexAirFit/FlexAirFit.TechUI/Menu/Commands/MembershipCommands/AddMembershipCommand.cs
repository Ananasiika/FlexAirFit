using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.MembershipCommands;

public class AddMembershipCommand : Command
{
    public override string? Description()
    {
        return "Добавить новый абонемент";
    }

    public override async Task Execute(Context context)
    {
        Console.Write("Введите название абонемента: ");
        string name = Console.ReadLine();

        Console.WriteLine("Введите продолжительность абонемента в формате (чч:мм:сс):");
        TimeSpan duration;
        if (!TimeSpan.TryParse(Console.ReadLine(), out duration))
        {
            Console.WriteLine("Ошибка: Неверный формат продолжительности абонемента.");
            return;
        }

        Console.Write("Введите цену: ");
        if (!int.TryParse(Console.ReadLine(), out int price))
        {
            Console.WriteLine("Ошибка: Неверный формат цены.");
            return;
        }

        Console.Write("Введите количество дней заморозки: ");
        if (!int.TryParse(Console.ReadLine(), out int freezing))
        {
            Console.WriteLine("Ошибка: Неверный формат количества дней.");
            return;
        }

        Membership membership = new(Guid.NewGuid(), name, duration, price, freezing);

        await context.MembershipService.CreateMembership(membership);
        Console.WriteLine("Абонемент успешно добавлен.");
    }
}