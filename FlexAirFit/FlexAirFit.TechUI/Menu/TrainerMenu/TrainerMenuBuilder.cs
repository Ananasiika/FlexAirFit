using FlexAirFit.TechnicalUI.GuestMenu.AuthActions;
using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.TechUI.Commands.ClientCommands;
using FlexAirFit.TechUI.Commands.ScheduleCommands;
using FlexAirFit.TechUI.Commands.TrainerCommands;
using FlexAirFit.TechUI.Commands.WorkoutCommands;
using FlexAirFit.TechUI.GuestMenu.AuthActions;

namespace FlexAirFit.TechUI.TrainerMenu;

public class TrainerMenuBuilder : MenuBuilder
{
    public override Menu BuildMenu()
    {
        Menu menu = new();
        menu.AddLabel(new("Действия с расписанием",
        [
            new AddRecordToScheduleCommand(),
            new ViewScheduleCommand(),
            new ViewScheduleWithFilterCommand(),
            new ViewOwnScheduleCommand()
        ]));
        menu.AddLabel(new("Действия с тренировками",
        [
            new AddWorkoutCommand(),
            new EditWorkoutCommand(),
            new ViewWorkoutCommand(),
            new ViewWorkoutWithFilterCommand()
        ]));
        menu.AddLabel(new("Просмотр информации о клиентах",
        [
            new ViewClientCommand()
        ]));
        menu.AddLabel(new("Изменение личной информации",
        [
            new EditTrainerCommand()
        ]));
        menu.AddLabel(new MenuLabel("Просмотр личной информации",
        [
            new ViewTrainerInformationCommand()
        ]));
        menu.AddLabel(new("Выйти из аккаунта",
        [
            new SignOutUserCommand()
        ]));
        return menu;
    }
}