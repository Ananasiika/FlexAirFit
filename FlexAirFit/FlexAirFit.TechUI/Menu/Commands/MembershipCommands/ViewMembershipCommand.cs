using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.MembershipCommands;

public class ViewMembershipCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewMembershipCommand>();

    public override string? Description()
    {
        return "Просмотр абонементов";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewMembershipCommand");

        var memberships = await context.MembershipService.GetMemberships(10, null);
        if (memberships.Count == 0)
        {
            _logger.Information("No memberships found");
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
                    _logger.Information("User exited from membership viewing");
                    return;
                }
            }

            foreach (var membership in memberships)
            {
                _logger.Information("Displaying membership: {@Membership}", membership);
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
