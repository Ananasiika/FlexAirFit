using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class ViewInformationAboutClientMembershipCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewInformationAboutClientMembershipCommand>();

    public override string? Description()
    {
        return "Просмотр информации об абонементе";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewInformationAboutClientMembershipCommand");

        Client client = await context.ClientService.GetClientById(context.CurrentUser.Id);
        Console.WriteLine($"Дата окончания абонемента: {client.MembershipEnd}");
        Console.WriteLine($"Количество оставшихся дней для заморозки: {client.RemainFreezing}");
    }
}