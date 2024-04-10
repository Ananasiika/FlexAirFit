using FlexAirFit.TechnicalUI.GuestMenu.AuthActions;
using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.TechUI.Commands.ScheduleCommands;
using FlexAirFit.TechUI.Commands.WorkoutCommands;
using FlexAirFit.TechUI.Commands.BonusCommands;
using FlexAirFit.TechUI.Commands.ClientCommands;
using FlexAirFit.TechUI.Commands.ClientProductCommands;
using FlexAirFit.TechUI.Commands.FreezingCommands;

namespace FlexAirFit.TechUI.ClientMenu;

public class ClientMenuBuilder : MenuBuilder
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
            new ViewWorkoutCommand(),
            new ViewWorkoutWithFilterCommand()
        ]));
        menu.AddLabel(new("Посмотреть количество своих бонусов",
        [
            new ViewClientBonusesCommand()
        ]));
        menu.AddLabel(new("Посмотреть информацию об абонементе",
        [
            new ViewInformationAboutClientMembershipCommand()
        ]));
        menu.AddLabel(new("Заморозить абонемент",
        [
            new FreezeMembershipCommand()
        ]));
        menu.AddLabel(new("Купить товар",
        [
            new BuyClientProductCommand()
        ]));
        menu.AddLabel(new("Выйти из аккаунта",
        [
            new SignOutUserCommand()
        ]));
        return menu;
    }
}