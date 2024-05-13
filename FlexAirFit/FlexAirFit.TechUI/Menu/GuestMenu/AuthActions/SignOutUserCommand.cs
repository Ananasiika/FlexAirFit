using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechnicalUI.GuestMenu.AuthActions;

public class SignOutUserCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<SignOutUserCommand>();
    public override string? Description()
    {
        return "Выйти из аккаунта";
    }

    public override async Task Execute(Context context)
    {
        context.CurrentUser = null;
        context.UserObject = null;
        _logger.Information("User signed out successfully");
    }
}

