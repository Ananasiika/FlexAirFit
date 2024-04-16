using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.TechUI.Commands.MembershipCommands;
using FlexAirFit.TechUI.Commands.ProductCommands;
using FlexAirFit.TechUI.Commands.ScheduleCommands;
using FlexAirFit.TechUI.Commands.TrainerCommands;
using FlexAirFit.TechUI.Commands.WorkoutCommands;
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
        menu.AddLabel(new("Действия с тренировками",
        [
            new ViewWorkoutCommand(),
            new ViewWorkoutWithFilterCommand()
        ]));
        menu.AddLabel(new("Просмотр информации о тренерах",
        [
            new ViewTrainerCommand()
        ]));
        menu.AddLabel(new("Просмотр товаров",
        [
            new ViewProductsCommand()
        ]));
        menu.AddLabel(new("Просмотр абонементов",
        [
            new ViewMembershipCommand()
        ]));
        return menu;
    }
}