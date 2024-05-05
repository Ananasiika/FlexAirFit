using FlexAirFit.TechnicalUI.GuestMenu.AuthActions;
using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.TechUI.Commands.ScheduleCommands;
using FlexAirFit.TechUI.Commands.WorkoutCommands;
using FlexAirFit.TechUI.Commands.BonusCommands;
using FlexAirFit.TechUI.Commands.ClientCommands;
using FlexAirFit.TechUI.Commands.FreezingCommands;
using FlexAirFit.TechUI.Commands.MembershipCommands;
using FlexAirFit.TechUI.Commands.ProductCommands;
using FlexAirFit.TechUI.Commands.TrainerCommands;

namespace FlexAirFit.TechUI.AdminMenu;

public class AdminMenuBuilder : MenuBuilder
{
    public override Menu BuildMenu()
    {
        Menu menu = new();
        menu.AddLabel(new("Действия с расписанием",
        [
            new AddRecordToScheduleCommand(),
            new DeleteRecordFromScheduleCommand(),
            new EditScheduleCommand(),
            new ViewScheduleCommand(),
            new ViewScheduleWithFilterCommand()
        ]));
        menu.AddLabel(new("Действия с тренировками",
        [
            new AddWorkoutCommand(),
            new EditWorkoutCommand(),
            new DeleteWorkoutCommand(),
            new ViewWorkoutCommand(),
            new ViewWorkoutWithFilterCommand()
        ]));
        menu.AddLabel(new("Действия с абонементами",
        [
            new AddMembershipCommand(),
            new EditMembershipCommand(),
            new DeleteMembershipCommand(),
            new ViewMembershipCommand()
        ]));
        menu.AddLabel(new("Действия с бонусами",
        [
            new EditClientBonusesCommand(),
            new ViewClientBonusesByAdminCommand()
        ]));
        menu.AddLabel(new("Действия с клиентами",
        [
            new ViewClientCommand(),
            new AddMembershipToClientCommand(),
            new EditClientByAdminCommand(),
            new FreezeMembershipByAdminCommand()
        ]));
        menu.AddLabel(new("Действия с тренерами",
        [
            new ViewTrainerCommand(),
            new EditTrainerByAdminCommand()
        ]));
        menu.AddLabel(new("Действия с товарами",
        [
            new AddProductCommand(),
            new DeleteProductCommand(),
            new EditProductCommand(),
            new ViewProductsCommand()
        ]));
        menu.AddLabel(new("Выйти из аккаунта",
        [
            new SignOutUserCommand()
        ]));
        return menu;
    }
}