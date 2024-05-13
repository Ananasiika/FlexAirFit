using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.MembershipCommands;

public class EditMembershipCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<EditMembershipCommand>();

    public override string? Description()
    {
        return "Изменить существующий абонемент";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing EditMembershipCommand");

        Console.WriteLine("Введите id абонемента: ");
        string membershipIdInput = Console.ReadLine();
        if (!Guid.TryParse(membershipIdInput, out Guid membershipId))
        {
            _logger.Warning("Invalid membership id format entered");
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id абонемента");
            return;
        }

        _logger.Information("Membership id entered: {MembershipId}", membershipId);

        if (!context.WorkoutService.CheckIfWorkoutExists(membershipId).Result)
        {
            _logger.Warning("Membership with id {MembershipId} does not exist", membershipId);
            Console.WriteLine("Ошибка: Абонемента с таким id не существует");
            return;
        }

        Console.Write("Введите новое название абонемента (или нажмите Enter, чтобы пропустить): ");
        string name = Console.ReadLine();
        _logger.Information("New membership name entered: {Name}", name);

        Console.WriteLine("Введите новую продолжительность абонемента в формате (чч:мм:сс) (или нажмите Enter, чтобы пропустить):");
        TimeSpan duration = TimeSpan.Zero;
        string durationInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(durationInput))
        {
            if (!TimeSpan.TryParse(durationInput, out duration))
            {
                _logger.Warning("Invalid membership duration format entered");
                Console.WriteLine("Ошибка: Неверный формат продолжительности абонемента.");
                return;
            }
        }

        _logger.Information("New membership duration entered: {Duration}", duration);

        Console.Write("Введите новую цену (или нажмите Enter, чтобы пропустить): ");
        int price = 0;
        string priceInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(priceInput))
        {
            if (!int.TryParse(priceInput, out price))
            {
                _logger.Warning("Invalid price format entered");
                Console.WriteLine("Ошибка: Неверный формат цены.");
                return;
            }
        }

        _logger.Information("New membership price entered: {Price}", price);

        Console.Write("Введите количество дней заморозки (или нажмите Enter, чтобы пропустить): ");
        int freezing = 0;
        string freezingInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(freezingInput))
        {
            if (!int.TryParse(freezingInput, out freezing))
            {
                _logger.Warning("Invalid freezing days format entered");
                Console.WriteLine("Ошибка: Неверный формат количества дней заморозки.");
                return;
            }
        }

        _logger.Information("New membership freezing days entered: {FreezingDays}", freezing);

        Membership membership = await context.MembershipService.GetMembershipById(membershipId);
        membership.Duration = (duration == TimeSpan.Zero) ? membership.Duration : duration;
        membership.Name = (string.IsNullOrEmpty(name)) ? membership.Name : name;
        membership.Freezing = (freezing == 0) ? membership.Freezing : freezing;
        membership.Price = (price == 0) ? membership.Price : price;

        try
        {
            await context.MembershipService.UpdateMembership(membership);
            Console.WriteLine("Абонемент успешно изменен.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while updating membership with id {MembershipId}", membershipId);
            throw;
        }
    }
}
