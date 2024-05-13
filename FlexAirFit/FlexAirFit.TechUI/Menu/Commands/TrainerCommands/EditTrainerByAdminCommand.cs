using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.TrainerCommands;

public class EditTrainerByAdminCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<EditTrainerByAdminCommand>();

    public override string? Description()
    {
        return "Изменить личную информацию тренера";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing EditTrainerByAdminCommand");

        Console.WriteLine("Введите ID тренера:");
        Guid idTrainer;
        if (!Guid.TryParse(Console.ReadLine(), out idTrainer))
        {
            _logger.Warning("Invalid trainer ID format entered");
            Console.WriteLine("Ошибка: Неверный формат ID тренера.");
            return;
        }

        if (!context.TrainerService.CheckIfTrainerExists(idTrainer).Result)
        {
            _logger.Warning("Trainer with ID {TrainerId} does not exist", idTrainer);
            Console.WriteLine("Ошибка: Тренера с таким id не существует");
            return;
        }

        _logger.Information("User entered trainer ID: {TrainerId}", idTrainer);

        Console.WriteLine("Введите новое имя (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();
        _logger.Information("User entered new name: {Name}", name);

        Console.WriteLine("Выберите новый пол (male, female) (или нажмите Enter, чтобы пропустить):");
        string gender = Console.ReadLine();
        _logger.Information("User entered new gender: {Gender}", gender);

        Console.WriteLine("Введите новую специализацию (или нажмите Enter, чтобы пропустить):");
        string specialization = Console.ReadLine();
        _logger.Information("User entered new specialization: {Specialization}", specialization);

        int experience = 0;
        Console.WriteLine("Введите новый опыт (или нажмите Enter, чтобы пропустить):");
        string experienceInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(experienceInput))
        {
            if (!int.TryParse(experienceInput, out experience))
            {
                _logger.Warning("Invalid experience format entered");
                Console.WriteLine("Ошибка: Неверный формат количества лет опыта");
                return;
            }
        }
        _logger.Information("User entered new experience: {Experience}", experience);

        int rating = 0;
        Console.WriteLine("Введите новый рейтинг (от 1 до 5) (или нажмите Enter, чтобы пропустить):");
        string ratingInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(ratingInput))
        {
            if (!int.TryParse(ratingInput, out rating))
            {
                _logger.Warning("Invalid rating format entered");
                Console.WriteLine("Ошибка: Неверный формат рейтинга");
                return;
            }
        }
        _logger.Information("User entered new rating: {Rating}", rating);

        Trainer trainer = await context.TrainerService.GetTrainerById(idTrainer);

        trainer.Rating = (rating == 0) ? trainer.Rating : rating;
        trainer.Specialization = (string.IsNullOrEmpty(specialization)) ? trainer.Specialization : specialization;
        trainer.Experience = (experience == 0) ? trainer.Experience : experience;
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
