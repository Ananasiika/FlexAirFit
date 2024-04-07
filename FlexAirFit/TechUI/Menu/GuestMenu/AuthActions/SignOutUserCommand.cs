using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechnicalUI.GuestMenu.AuthActions;

public class SignOutUserCommand : Command
{
    public override string? Description()
    {
        return "Выйти из аккаунта";
    }

    public override async Task Execute(Context context)
    {
        context.CurrentUser = null;
        context.UserObject = null;
    }
}

