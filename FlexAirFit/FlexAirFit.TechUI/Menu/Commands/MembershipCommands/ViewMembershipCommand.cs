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
        var memberships = await context.MembershipService.GetMemberships(10, null);
        if (memberships.Count == 0)
        {
            Console.WriteLine("Нет абонементов.");
            return;
        }
        
        Console.WriteLine("Список абонементов:");
        int page = 1;
        while (memberships.Count != 0)
        {
            if (page != 1)
            {
                Console.WriteLine("Введите 0, чтобы закончить или Enter, чтобы продолжить:");
                string nextInput = Console.ReadLine();
                if (!int.TryParse(nextInput, out int next))
                {
                    next = 1;
                }

                if (next == 0)
                {
                    return;
                }
            }
            foreach (var membership in memberships)
            {
                Console.WriteLine($"ID: {membership.Id}");
                Console.WriteLine($"Название: {membership.Name}");
                Console.WriteLine($"Длительность: {membership.Duration}");
                Console.WriteLine($"Цена: {membership.Price}");
                Console.WriteLine($"Заморозка: {membership.Freezing} дней");
                Console.WriteLine();
            }
            memberships = await context.MembershipService.GetMemberships(10, 10 * page++);
        }
    }
}