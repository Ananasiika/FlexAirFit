using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class ViewClientCommand : Command
{
    public override string? Description()
    {
        return "Просмотр клиентов";
    }

    public override async Task Execute(Context context)
    {
        var clients = await context.ClientService.GetClients(null, null);

        if (clients.Count == 0)
        {
            Console.WriteLine("Нет клиентов.");
        }
        else
        {
            Console.WriteLine("Список клиентов:");

            foreach (var client in clients)
            {
                Console.WriteLine($"ID: {client.Id}");
                Console.WriteLine($"ID пользователя: {client.IdUser}");
                Console.WriteLine($"Имя: {client.Name}");
                Console.WriteLine($"Пол: {client.Gender}");
                Console.WriteLine($"Дата рождения: {client.DateOfBirth}");
                Console.WriteLine($"ID абонемента: {client.IdMembership}");
                Console.WriteLine($"Окончание абонемента: {client.MembershipEnd}");
                Console.WriteLine($"Осталось заморозок: {(client.RemainFreezing.HasValue ? client.RemainFreezing.ToString() : "Нет информации")}");
            
                if (client.FreezingIntervals != null && client.FreezingIntervals.Count > 0)
                {
                    Console.WriteLine("Интервалы заморозки:");
                    foreach (var interval in client.FreezingIntervals)
                    {
                        Console.WriteLine($"Начало: {interval.Item1}, Конец: {interval.Item2}");
                    }
                }
            
                Console.WriteLine();
            }
        }
    }
}