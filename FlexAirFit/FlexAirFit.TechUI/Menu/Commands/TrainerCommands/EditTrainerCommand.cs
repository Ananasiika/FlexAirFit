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
            Console.WriteLine("При изменении произошла ошибка", e);
            return;
        }
        
        Console.WriteLine("Личная информация успешно изменена.");
    }
}