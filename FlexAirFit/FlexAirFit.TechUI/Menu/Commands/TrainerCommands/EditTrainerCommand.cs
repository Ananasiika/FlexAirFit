using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.TrainerCommands;

public class EditTrainerCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<EditTrainerCommand>();

    public override string? Description()
    {
        return "Изменить личную информацию";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing EditTrainerCommand for user {UserId}", context.CurrentUser.Id);

        Console.WriteLine("Введите ваше новое имя (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();
        _logger.Information("User entered new name: {Name}", name);

        Console.WriteLine("Выберите ваш новый пол (male, female) (или нажмите Enter, чтобы пропустить):");
        string gender = Console.ReadLine();
        _logger.Information("User entered new gender: {Gender}", gender);

        Console.WriteLine("Введите вашу новую специализацию (или нажмите Enter, чтобы пропустить):");
        string specialization = Console.ReadLine();
        _logger.Information("User entered new specialization: {Specialization}", specialization);

        Trainer trainer = await context.TrainerService.GetTrainerById(context.CurrentUser.Id);

        trainer.Specialization = (string.IsNullOrEmpty(specialization)) ? trainer.Specialization : specialization;
        trainer.Gender = (string.IsNullOrEmpty(gender)) ? trainer.Gender : gender;
        trainer.Name = (string.IsNullOrEmpty(name)) ? trainer.Name : name;

        try
        {
            await context.TrainerService.UpdateTrainer(trainer);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error updating trainer");
            Console.WriteLine("При изменении произошла ошибка", e);
            return;
        }

        Console.WriteLine("Личная информация успешно изменена.");
    }
}
