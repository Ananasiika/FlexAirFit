using FlexAirFit.TechnicalUI.GuestMenu.AuthActions;
using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.TechUI.Commands.ScheduleCommands;
using FlexAirFit.TechUI.Commands.WorkoutCommands;
using FlexAirFit.TechUI.Commands.BonusCommands;
using FlexAirFit.TechUI.Commands.ClientCommands;
using FlexAirFit.TechUI.Commands.ClientProductCommands;
using FlexAirFit.TechUI.Commands.FreezingCommands;
using FlexAirFit.TechUI.Commands.MembershipCommands;
using FlexAirFit.TechUI.Commands.ProductCommands;
using FlexAirFit.TechUI.Commands.TrainerCommands;

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
            new ViewScheduleWithFilterCommand(),
            new ViewOwnScheduleCommand()
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
        menu.AddLabel(new("Просмотр всех возможных абонементов",
        [
            new ViewMembershipCommand()
        ]));
        menu.AddLabel(new("Просмотр всех товаров",
        [
            new ViewProductsCommand()
        ]));
        menu.AddLabel(new("Купить товар",
        [
            new BuyClientProductCommand()
        ]));
        menu.AddLabel(new("Изменение личной информации",
        [
            new EditClientCommand()
        ]));
        menu.AddLabel(new("Выйти из аккаунта",
        [
            new SignOutUserCommand()
        ]));
        return menu;
    }
}