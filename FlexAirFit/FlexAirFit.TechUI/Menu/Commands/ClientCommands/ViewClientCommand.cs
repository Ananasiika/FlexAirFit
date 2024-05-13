using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class ViewClientCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewClientCommand>();

    public override string? Description()
    {
        return "Просмотр клиентов";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewClientCommand");

        var clients = await context.ClientService.GetClients(10, null);
        _logger.Information($"Retrieved {clients.Count} clients");

        if (clients.Count == 0)
        {
            _logger.Information("No clients found");
            Console.WriteLine("Нет клиентов.");
            return;
        }

        Console.WriteLine("Список клиентов:");
        int page = 1;
        while (clients.Count != 0)
        {
            if (page != 1)
            {
                Console.WriteLine("Введите 0, чтобы закончить или Enter, чтобы продолжить:");
                string nextInput = Console.ReadLine();
                _logger.Information("User input: {NextInput}", nextInput);

                if (!int.TryParse(nextInput, out int next))
                {
                    next = 1;
                }

                if (next == 0)
                {
                    _logger.Information("User chose to exit");
                    return;
                }
            }

            foreach (var client in clients)
            {
                _logger.Information("Displaying client: {ClientId}", client.Id);
                Console.WriteLine($"ID: {client.Id}");
                Console.WriteLine($"Имя: {client.Name}");
                Console.WriteLine($"Пол: {client.Gender}");
                Console.WriteLine($"Дата рождения: {client.DateOfBirth}");
                Console.WriteLine($"ID абонемента: {client.IdMembership}");
                Console.WriteLine($"Окончание абонемента: {client.MembershipEnd}");
                Console.WriteLine($"Осталось заморозок: {(client.RemainFreezing.HasValue ? client.RemainFreezing.ToString() : "Нет информации")}");

                if (client.FreezingIntervals != null && client.FreezingIntervals.Length > 0)
                {
                    Console.WriteLine("Интервалы заморозки:");
                    foreach (var interval in client.FreezingIntervals)
                    {
                        Console.WriteLine($"Начало: {interval[0]}, Конец: {interval[1]}");
                    }
                }

                Console.WriteLine();
            }

            clients = await context.ClientService.GetClients(10, 10 * page++);
        }
    }
}
