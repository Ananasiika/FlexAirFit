using FlexAirFit.Core.Models;
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
        if (days < 7)
        {
            Console.WriteLine("Ошибка: Нельзя заморозить абонемент менее, чем на 7 дней.");
            return;
        }
        
        Console.WriteLine("Введите дату начала заморозки YYYY-MM-DD:");
        if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly dateStartFreezing))
        {
            Console.WriteLine("Ошибка: Неверный формат даты начала заморозки");
            return;
        }
        if (dateStartFreezing <= DateOnly.FromDateTime(DateTime.Today))
        {
            Console.WriteLine("Ошибка: Дата начала заморозки не может быть ранее, чем сегодня.");
            return;
        }
        
        try
        {
            await context.ClientService.FreezeMembership(context.CurrentUser.Id, dateStartFreezing, days);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
            else
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }
}