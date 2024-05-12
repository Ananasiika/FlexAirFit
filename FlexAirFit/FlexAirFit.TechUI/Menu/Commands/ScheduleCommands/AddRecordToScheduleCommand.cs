using FlexAirFit.Core;
using FlexAirFit.Core.Enums;
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
        
        if (context.CurrentUser.Role == UserRole.Trainer)
        {
            Workout workout = await context.WorkoutService.GetWorkoutById(workoutId);
            if (workout.IdTrainer != context.CurrentUser.Id)
            {
                Console.WriteLine("Ошибка: Тренировка не принадлежит текущему тренеру");
                return;
            }
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

        Guid? clientId = null;
        if (context.CurrentUser.Role != UserRole.Client)
        {
            Console.Write("Введите id клиента (если тренировка групповая, нажмите Enter): ");
            string clientIdInput = Console.ReadLine();
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
        }
        else
        {
            clientId = context.CurrentUser.Id;
        }

        Schedule newScheduleItem = new Schedule(Guid.NewGuid(), workoutId, utcDateTime, clientId);
        
        try
        {
            await context.ScheduleService.CreateSchedule(newScheduleItem);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Произошла ошибка при добавлении записи в расписание.");;
            return;
        }
        Console.WriteLine("Запись успешно добавлена в расписание.");
    }
}