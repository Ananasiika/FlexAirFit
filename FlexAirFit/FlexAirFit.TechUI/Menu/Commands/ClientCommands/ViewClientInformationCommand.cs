using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class ViewInformationAboutClientCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewInformationAboutClientCommand>();

    public override string? Description()
    {
        return "Просмотр личной информации";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewInformationAboutClientCommand");

        var client = await context.ClientService.GetClientById(context.CurrentUser.Id);

        Console.WriteLine($"Имя: {client.Name}");
        Console.WriteLine($"Пол: {client.Gender}");
        Console.WriteLine($"Дата рождения: {client.DateOfBirth.ToShortDateString()}");
        Console.WriteLine($"ID абонемента: {client.IdMembership}");
        Console.WriteLine($"Дата окончания абонемента: {client.MembershipEnd}");
        Console.WriteLine($"Количество оставшихся дней для заморозки: {client.RemainFreezing}");

        if (client.FreezingIntervals == null || client.FreezingIntervals.Length == 0)
        {
            Console.WriteLine("Записанных периодов заморозки нет");
        }
        else
        {
            Console.WriteLine("Периоды заморозок:");
            foreach (var interval in client.FreezingIntervals)
            {
                Console.WriteLine($"{interval[0].ToString()} - {interval[1].ToString()}");
            }
        }

        Console.WriteLine();
    }
}