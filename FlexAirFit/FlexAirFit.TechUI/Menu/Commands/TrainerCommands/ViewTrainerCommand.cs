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
        var trainers = await context.TrainerService.GetTrainers(10, null);
        if (trainers.Count == 0)
        {
            Console.WriteLine("Нет тренеров.");
            return;
        }
        
        Console.WriteLine("Список тренеров:");
        int page = 1;
        while (trainers.Count != 0)
        {
            if (page != 1)
            {
                Console.WriteLine("Введите 0, чтобы закончить или Enter, чтобы продолжить:");
                string nextInput = Console.ReadLine();
                if (!int.TryParse(nextInput, out int next))
                {
                    next = 1;
                }

                if (next == 0)
                {
                    return;
                }
            }
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
            trainers = await context.TrainerService.GetTrainers(10, 10 * page++);
        }
    }
}