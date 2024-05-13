using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.TrainerCommands;

public class ViewTrainerInformationCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewTrainerInformationCommand>();

    public override string? Description()
    {
        return "Просмотр личной информации";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing ViewTrainerInformationCommand for user {UserId}", context.CurrentUser.Id);

        var trainer = await context.TrainerService.GetTrainerById(context.CurrentUser.Id);

        Console.WriteLine($"Имя: {trainer.Name}");
        Console.WriteLine($"Пол: {trainer.Gender}");
        Console.WriteLine($"Специализация: {trainer.Specialization}");
        Console.WriteLine($"Опыт: {trainer.Experience}");
        Console.WriteLine($"Рейтинг: {trainer.Rating}");
        Console.WriteLine();
    }
}