using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.TrainerCommands;

public class ViewTrainerCommand : Command
{
    public override string? Description()
    {
        return "Просмотр тренеров";
    }

    public override async Task Execute(Context context)
    {
        var trainers = await context.TrainerService.GetTrainers(null, null);

        if (trainers.Count == 0)
        {
            Console.WriteLine("Нет тренеров.");
        }
        else
        {
            Console.WriteLine("Список тренеров:");

            foreach (var trainer in trainers)
            {
                Console.WriteLine($"ID: {trainer.Id}");
                Console.WriteLine($"Имя: {trainer.Name}");
                Console.WriteLine($"Пол: {trainer.Gender}");
                Console.WriteLine($"Специализация: {trainer.Specialization}");
                Console.WriteLine($"Опыт работы: {trainer.Experience} лет");
                Console.WriteLine($"Рейтинг: {trainer.Rating}");
                Console.WriteLine();
            }
        }
    }
}