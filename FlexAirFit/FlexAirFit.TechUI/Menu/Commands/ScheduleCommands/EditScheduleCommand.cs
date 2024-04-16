using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class EditScheduleCommand : Command
{
    public override string? Description()
    {
        return "Изменить существующую запись в расписании";
    }
    
    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите id записи: ");
        string scheduleIdInput = Console.ReadLine();
        if (!Guid.TryParse(scheduleIdInput, out Guid scheduleId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id записи");
            return;
        }
        if (!context.ScheduleService.CheckIfScheduleExists(scheduleId).Result)
        {
            Console.WriteLine("Ошибка: Записи с таким id не существует");
            return;
        }
        
        Console.Write("Введите новый id тренировки (или нажмите Enter, чтобы пропустить): ");
        string workoutIdInput = Console.ReadLine();
        Guid workoutId = Guid.Empty;
        if (!string.IsNullOrEmpty(workoutIdInput))
        {
            if (!Guid.TryParse(Console.ReadLine(), out workoutId))
            {
                Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id тренировки");
                return;
            }
            if (!context.WorkoutService.CheckIfWorkoutExists(workoutId).Result)
            {
             Console.WriteLine("Ошибка: Тренировки с таким id не существует");
             return;
            }
        }
        
        Console.Write("Введите новую дату и время в формате (гггг-мм-дд чч:мм) или нажмите Enter, чтобы пропустить: ");
        DateTime utcDateTime = DateTime.MinValue;
        string dateAndTimeInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(dateAndTimeInput))
        {
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

            utcDateTime = dateAndTime.ToUniversalTime();
        }
        
        Console.Write("Введите новый id клиента (если тренировка групповая, нажмите Enter): ");
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

        Schedule schedule = await context.ScheduleService.GetScheduleById(scheduleId);
        schedule.IdWorkout = (workoutId == Guid.Empty) ? schedule.IdWorkout : workoutId;
        schedule.DateAndTime = (utcDateTime == DateTime.MinValue) ? schedule.DateAndTime : utcDateTime;
        schedule.IdClient = clientId;
        
        await context.ScheduleService.UpdateSchedule(schedule);
        Console.WriteLine("Запись успешно изменена.");
    }
}