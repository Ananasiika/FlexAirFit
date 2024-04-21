using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.BonusCommands;

public class ViewClientBonusesCommand : Command
{
    public override string? Description()
    {
        return "Просмотр количества бонусов";
    }

    public override async Task Execute(Context context)
    {
        int bonuses = await context.BonusService.GetCountBonusByIdClient(context.CurrentUser.Id);
        Console.WriteLine($"Бонусов у текущего клиента: {bonuses}");
    }
}