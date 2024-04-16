using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.MembershipCommands;

public class DeleteMembershipCommand : Command
{
    public override string? Description()
    {
        return "Удалить существующий абонемент";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите id абонемента: ");
        string membershipIdInput = Console.ReadLine();
        if (!Guid.TryParse(membershipIdInput, out Guid membershipId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id абонемента");
            return;
        }

        if (!context.MembershipService.CheckIfMembershipExists(membershipId).Result)
        {
            Console.WriteLine("Ошибка: Абонемента с таким id не существует");
            return;
        }
        
        try
        {
            await context.MembershipService.DeleteMembership(membershipId);
        }
        catch (Exception e)
        {
            Console.WriteLine("При удалении произошла ошибка", e);
            return;
        }
        
        Console.WriteLine("Абонемент успешно удален");
    }
}