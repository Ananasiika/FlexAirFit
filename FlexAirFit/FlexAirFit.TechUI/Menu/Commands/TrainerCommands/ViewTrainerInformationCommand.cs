using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.TrainerCommands;

public class ViewTrainerInformationCommand : Command
{
    public override string? Description()
    {
        return "Просмотр личной информации";
    }

    public override async Task Execute(Context context)
    {
        var trainer = await context.TrainerService.GetTrainerById(context.CurrentUser.Id);
        Console.WriteLine($"Имя: {trainer.Name}");
        Console.WriteLine($"Пол: {trainer.Gender}");
        Console.WriteLine($"Специализация: {trainer.Specialization}");
        Console.WriteLine($"Опыт: {trainer.Experience}");
        Console.WriteLine($"Рейтинг: {trainer.Rating}");
        Console.WriteLine();
    }
}