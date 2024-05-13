using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.MembershipCommands;

public class AddMembershipCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<AddMembershipCommand>();

    public override string? Description()
    {
        return "Добавить новый абонемент";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing AddMembershipCommand");

        Console.Write("Введите название абонемента: ");
        string name = Console.ReadLine();
        _logger.Information("Membership name entered: {Name}", name);

        Console.WriteLine("Введите продолжительность абонемента в формате (чч:мм:сс):");
        TimeSpan duration;
        if (!TimeSpan.TryParse(Console.ReadLine(), out duration))
        {
            _logger.Warning("Invalid membership duration format entered");
            Console.WriteLine("Ошибка: Неверный формат продолжительности абонемента.");
            return;
        }

        _logger.Information("Membership duration entered: {Duration}", duration);

        Console.Write("Введите цену: ");
        if (!int.TryParse(Console.ReadLine(), out int price))
        {
            _logger.Warning("Invalid price format entered");
            Console.WriteLine("Ошибка: Неверный формат цены.");
            return;
        }

        _logger.Information("Membership price entered: {Price}", price);

        Console.Write("Введите количество дней заморозки: ");
        if (!int.TryParse(Console.ReadLine(), out int freezing))
        {
            _logger.Warning("Invalid freezing days format entered");
            Console.WriteLine("Ошибка: Неверный формат количества дней.");
            return;
        }

        _logger.Information("Membership freezing days entered: {FreezingDays}", freezing);

        Membership membership = new(Guid.NewGuid(), name, duration, price, freezing);

        try
        {
            await context.MembershipService.CreateMembership(membership);
            Console.WriteLine("Абонемент успешно добавлен.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while creating membership: {@Membership}", membership);
            throw;
        }
    }
}
