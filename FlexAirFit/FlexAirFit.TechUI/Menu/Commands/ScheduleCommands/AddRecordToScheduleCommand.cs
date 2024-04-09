using FlexAirFit.Core;
using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class AddRecordToScheduleCommand : Command
{
    public override string? Description()
    {
        return "Добавить запись в расписание";
    }

    public override async Task Execute(Context context)
    {
        Console.Write("Введите id тренировки: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid workoutId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id тренировки");
            return;
        }

        if (!context.WorkoutService.CheckIfWorkoutExists(workoutId).Result)
        {
            Console.WriteLine("Ошибка: Тренировки с таким id не существует");
            return;
        }

        Console.Write("Введите дату и время в формате (гггг-мм-дд чч:мм): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateAndTime))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для даты и времени");
            return;
        }

        if (dateAndTime < DateTime.Now)
        {
            Console.WriteLine("Ошибка: Введенные дата и время должны быть не в прошлом...");
            return;
        }
        DateTime utcDateTime = dateAndTime.ToUniversalTime();

        Console.Write("Введите id клиента (если тренировка групповая, нажмите Enter): ");
        string clientIdInput = Console.ReadLine();
        Guid? clientId = null;
        if (!string.IsNullOrWhiteSpace(clientIdInput))
        {
            if (!Guid.TryParse(clientIdInput, out Guid tempClientId))
            {
                Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id клиента");
                return;
            }

            if (!context.ClientService.CheckIfClientExists(tempClientId).Result)
            {
                Console.WriteLine("Ошибка: Клиента с таким id не существует");
                return;
            }

            clientId = tempClientId;
        }

        Schedule newScheduleItem = new Schedule(Guid.NewGuid(), workoutId, utcDateTime, clientId);

        await context.ScheduleService.CreateSchedule(newScheduleItem);
        Console.WriteLine("Запись успешно добавлена в расписание.");
    }
}