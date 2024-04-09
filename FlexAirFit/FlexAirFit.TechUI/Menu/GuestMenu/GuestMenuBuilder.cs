using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.TechUI.Commands.ScheduleCommands;
using FlexAirFit.TechUI.GuestMenu.AuthActions;

namespace FlexAitFit.TechUI.GuestMenu;

public class GuestMenuBuilder : MenuBuilder
{
    public override Menu BuildMenu()
    {
        Menu menu = new();
        menu.AddLabel(new("Зарегистрироваться",
        [
            new RegisterUserCommand()
        ]));
        menu.AddLabel(new("Авторизоваться",
        [
            new SignInUserCommand()
        ]));
        menu.AddLabel(new("Действия с расписанием",
        [
            new ViewScheduleCommand(),
            new ViewScheduleWithFilterCommand()
        ]));
        return menu;
    }
}