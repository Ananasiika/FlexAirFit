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
        
        Console.WriteLine("Введите дату начала заморозки YYYY-MM-DD:");
        if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly dateStartFreezing))
        {
            Console.WriteLine("Ошибка: Неверный формат даты начала заморозки");
            return;
        }
        
        Client client = await context.ClientService.GetClientByIdUser(context.CurrentUser.Id);
        try
        {
            await context.ClientService.FreezeMembership(client.Id, dateStartFreezing, days);
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