using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class ViewInformationAboutClientMembershipCommand : Command
{
    public override string? Description()
    {
        return "Просмотр информации об абонементе";
    }

    public override async Task Execute(Context context)
    {
        Client client = await context.ClientService.GetClientById(context.CurrentUser.Id);
        Console.WriteLine($"Дата окончания абонемента: {client.MembershipEnd}");
        Console.WriteLine($"Количество оставшихся дней для заморозки: {client.RemainFreezing}");
    }
}