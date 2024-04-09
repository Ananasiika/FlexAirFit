using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.FreezingCommands;

public class FreezeMembershipCommand : Command
{
    public override string? Description()
    {
        return "Заморозить абонемент";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите количество дней, на которое хотите заморозить абонемент:");
        string daysInput = Console.ReadLine();
        if (!int.TryParse(daysInput, out int days))
        {
            Console.WriteLine("Ошибка: Неверный формат количества дней для заморозки.");
            return;
        }
        DateOnly dateStartFreezing;
        Console.WriteLine("Введите дату начала заморозки YYYY-MM-DD:");
        if (!DateOnly.TryParse(Console.ReadLine(), out dateStartFreezing))
        {
            Console.WriteLine("Ошибка: Неверный формат даты начала заморозки");
            return;
        }

        await context.ClientService.FreezeMembership(context.CurrentUser.Id, dateStartFreezing, days);
    }
}