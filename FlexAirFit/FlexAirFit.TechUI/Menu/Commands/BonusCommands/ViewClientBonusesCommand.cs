using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.BonusCommands;

public class ViewClientBonusesCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewClientBonusesCommand>();
    public override string? Description()
    {
        return "Просмотр количества бонусов";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewClientBonusesCommand");
        int bonuses = await context.BonusService.GetCountBonusByIdClient(context.CurrentUser.Id);
        Console.WriteLine($"Бонусов у текущего клиента: {bonuses}");
    }
}