using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.MembershipCommands;

public class ViewMembershipCommand : Command
{
    public override string? Description()
    {
        return "Просмотр абонементов";
    }

    public override async Task Execute(Context context)
    {
        var memberships = await context.MembershipService.GetMemberships(null, null);

        if (memberships.Count == 0)
        {
            Console.WriteLine("Нет абонементов.");
        }
        else
        {
            Console.WriteLine("Список абонементов:");

            foreach (var membership in memberships)
            {
                Console.WriteLine($"ID: {membership.Id}");
                Console.WriteLine($"Название: {membership.Name}");
                Console.WriteLine($"Длительность: {membership.Duration}");
                Console.WriteLine($"Цена: {membership.Price}");
                Console.WriteLine($"Заморозка: {membership.Freezing} дней");
                Console.WriteLine();
            }
        }
    }
}