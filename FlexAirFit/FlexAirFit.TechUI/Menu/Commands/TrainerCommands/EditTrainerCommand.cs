using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.TrainerCommands;

public class EditTrainerCommand : Command
{
    public override string? Description()
    {
        return "Изменить личную информацию";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите ваше новое имя (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();

        Console.WriteLine("Выберите ваш новый пол (male, female) (или нажмите Enter, чтобы пропустить):");
        string gender = Console.ReadLine();
        
        Console.WriteLine("Введите вашу новую специализацию (или нажмите Enter, чтобы пропустить):");
        string specialization = Console.ReadLine();

        int experience = 0;
        Console.WriteLine("Введите ваш новый опыт (или нажмите Enter, чтобы пропустить):");
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
        Console.WriteLine("Введите ваш новый рейтинг (от 1 до 5):");
        string ratingInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(ratingInput))
        {
            if (!int.TryParse(Console.ReadLine(), out rating))
            {
                Console.WriteLine("Ошибка: Неверный формат рейтинга");
                return;
            }
        }

        Trainer trainer = await context.TrainerService.GetTrainerByIdUser(context.CurrentUser.Id);
        
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