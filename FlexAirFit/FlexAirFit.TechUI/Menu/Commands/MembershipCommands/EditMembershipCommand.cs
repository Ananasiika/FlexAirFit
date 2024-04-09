using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.MembershipCommands;

public class EditMembershipCommand : Command
{
    public override string? Description()
    {
        return "Изменить существующий абонемент";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите id абонемента: ");
        string membershipIdInput = Console.ReadLine();
        if (!Guid.TryParse(membershipIdInput, out Guid membershipId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id абонемента");
            return;
        }
        if (!context.WorkoutService.CheckIfWorkoutExists(membershipId).Result)
        {
            Console.WriteLine("Ошибка: Абонемента с таким id не существует");
            return;
        }
        
        Console.Write("Введите новое название абонемента (или нажмите Enter, чтобы пропустить): ");
        string name = Console.ReadLine();

        Console.WriteLine("Введите новую продолжительность абонемента в формате (чч:мм:сс) (или нажмите Enter, чтобы пропустить):");
        TimeSpan duration = TimeSpan.Zero;
        string durationInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(durationInput))
        {
            if (!TimeSpan.TryParse(durationInput, out duration))
            {
                Console.WriteLine("Ошибка: Неверный формат продолжительности абонемента.");
                return;
            }
        }

        Console.Write("Введите новую цену: ");
        int price = 0;
        string priceInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(priceInput))
        {
            if (!int.TryParse(priceInput, out price))
            {
                Console.WriteLine("Ошибка: Неверный формат цены.");
                return;
            }
        }

        Console.Write("Введите количество дней заморозки: ");
        int freezing = 0;
        string freezingInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(freezingInput))
        {
            if (!int.TryParse(freezingInput, out freezing))
            {
                Console.WriteLine("Ошибка: Неверный формат количества дней заморозки.");
                return;
            }
        }
        
        await context.MembershipService.UpdateMembership(membershipId, name, duration, freezing, price);
        Console.WriteLine("Абонемент успешно изменен.");
    }
}
