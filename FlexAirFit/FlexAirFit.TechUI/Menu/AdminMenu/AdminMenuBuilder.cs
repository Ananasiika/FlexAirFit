using FlexAirFit.TechnicalUI.GuestMenu.AuthActions;
using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.TechUI.Commands.ScheduleCommands;
using FlexAirFit.TechUI.Commands.WorkoutCommands;
using FlexAirFit.TechUI.Commands.BonusCommands;

namespace FlexAirFit.TechUI.AdminMenu;

public class AdminMenuBuilder : MenuBuilder
{
    public override Menu BuildMenu()
    {
        Menu menu = new();
        menu.AddLabel(new("Действия с расписанием",
        [
            new AddRecordToScheduleCommand(),
            new ViewScheduleCommand(),
            new ViewScheduleWithFilterCommand()
        ]));
        menu.AddLabel(new("Действия с тренировками",
        [
            new AddWorkoutCommand(),
            new EditWorkoutCommand(),
            new ViewWorkoutCommand(),
            new ViewWorkoutWithFilterCommand()
        ]));
        menu.AddLabel(new("Действия с бонусами",
        [
            new EditClientBonusesCommand(),
            new ViewClientBonusesCommand()
        ]));
        menu.AddLabel(new("Выйти из аккаунта",
        [
            new SignOutUserCommand()
        ]));
        return menu;
    }
}