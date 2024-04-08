using FlexAirFit.TechnicalUI.GuestMenu.AuthActions;
using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.TechUI.Commands.ScheduleCommands;

namespace FlexAirFit.TechUI.ClientMenu;

public class ClientMenuBuilder : MenuBuilder
{
    public override Menu BuildMenu()
    {
        Menu menu = new();
        menu.AddLabel(new("Действия с расписанием",
        [
            new AddRecordToScheduleCommand(),
        ]));
        menu.AddLabel(new("Выйти из аккаунта",
        [
            new SignOutUserCommand()
        ]));
        return menu;
    }
}