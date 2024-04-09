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
        Console.WriteLine($"Дата окончания абонемента: {context.CurrentUser.MembershipEnd}");
        Console.WriteLine($"Количество оставшихся дней для заморозки: {context.CurrentUser.FreezingRemain}");
    }
}
