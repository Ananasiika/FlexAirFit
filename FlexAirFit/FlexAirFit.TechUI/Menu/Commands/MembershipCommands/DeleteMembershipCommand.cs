using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.MembershipCommands;

public class DeleteMembershipCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<DeleteMembershipCommand>();

    public override string? Description()
    {
        return "Удалить существующий абонемент";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing DeleteMembershipCommand");

        Console.WriteLine("Введите id абонемента: ");
        string membershipIdInput = Console.ReadLine();
        if (!Guid.TryParse(membershipIdInput, out Guid membershipId))
        {
            _logger.Warning("Invalid membership id format entered");
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id абонемента");
            return;
        }

        _logger.Information("Membership id entered: {MembershipId}", membershipId);

        if (!context.MembershipService.CheckIfMembershipExists(membershipId).Result)
        {
            _logger.Warning("Membership with id {MembershipId} does not exist", membershipId);
            Console.WriteLine("Ошибка: Абонемента с таким id не существует");
            return;
        }

        try
        {
            await context.MembershipService.DeleteMembership(membershipId);
            Console.WriteLine("Абонемент успешно удален");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while deleting membership with id {MembershipId}", membershipId);
            Console.WriteLine("При удалении произошла ошибка", ex);
        }
    }
}