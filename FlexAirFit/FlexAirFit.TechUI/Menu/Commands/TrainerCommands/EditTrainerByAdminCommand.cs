using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.TrainerCommands;

public class EditTrainerByAdminCommand : Command
{
    public override string? Description()
    {
        return "Изменить личную информацию тренера";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите ID тренера:");
        Guid idTrainer;
        if (!Guid.TryParse(Console.ReadLine(), out idTrainer))
        {
            Console.WriteLine("Ошибка: Неверный формат ID тренера.");
            return;
        }
        if (!context.TrainerService.CheckIfTrainerExists(idTrainer).Result)
        {
            Console.WriteLine("Ошибка: Тренера с таким id не существует");
            return;
        }
        
        Console.WriteLine("Введите новое имя (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();

        Console.WriteLine("Выберите новый пол (male, female) (или нажмите Enter, чтобы пропустить):");
        string gender = Console.ReadLine();
        
        Console.WriteLine("Введите новую специализацию (или нажмите Enter, чтобы пропустить):");
        string specialization = Console.ReadLine();

        int experience = 0;
        Console.WriteLine("Введите новый опыт (или нажмите Enter, чтобы пропустить):");
        string experienceInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(experienceInput))
        {
            if (!int.TryParse(Console.ReadLine(), out experience))
            {
                Console.WriteLine("Ошибка: Неверный формат количества лет опыта");
                return;
            }
        }
        
        int rating = 0;
        Console.WriteLine("Введите новый рейтинг (от 1 до 5) (или нажмите Enter, чтобы пропустить):");
        string ratingInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(ratingInput))
        {
            if (!int.TryParse(Console.ReadLine(), out rating))
            {
                Console.WriteLine("Ошибка: Неверный формат рейтинга");
                return;
            }
        }

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
            Console.WriteLine("При изменении произошла ошибка", e);
            return;
        }
        
        Console.WriteLine("Личная информация успешно изменена.");
    }
}