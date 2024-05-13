using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class EditScheduleCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<EditScheduleCommand>();

    public override string? Description()
    {
        return "Изменить существующую запись в расписании";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing EditScheduleCommand");

        Console.WriteLine("Введите id записи: ");
        string scheduleIdInput = Console.ReadLine();
        if (!Guid.TryParse(scheduleIdInput, out Guid scheduleId))
        {
            _logger.Warning("Invalid schedule id format entered: {ScheduleIdInput}", scheduleIdInput);
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id записи");
            return;
        }

        _logger.Information("Schedule id entered: {ScheduleId}", scheduleId);

        if (!context.ScheduleService.CheckIfScheduleExists(scheduleId).Result)
        {
            _logger.Warning("Schedule with id {ScheduleId} does not exist", scheduleId);
            Console.WriteLine("Ошибка: Записи с таким id не существует");
            return;
        }

        _logger.Information("Schedule with id {ScheduleId} exists, proceeding to edit", scheduleId);

        Console.Write("Введите новый id тренировки (или нажмите Enter, чтобы пропустить): ");
        string workoutIdInput = Console.ReadLine();
        Guid workoutId = Guid.Empty;
        if (!string.IsNullOrEmpty(workoutIdInput))
        {
            if (!Guid.TryParse(Console.ReadLine(), out workoutId))
            {
                _logger.Warning("Invalid workout id format entered: {WorkoutIdInput}", workoutIdInput);
                Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id тренировки");
                return;
            }

            _logger.Information("New workout id entered: {WorkoutId}", workoutId);

            if (!context.WorkoutService.CheckIfWorkoutExists(workoutId).Result)
            {
                _logger.Warning("Workout with id {WorkoutId} does not exist", workoutId);
                Console.WriteLine("Ошибка: Тренировки с таким id не существует");
                return;
            }

            _logger.Information("Workout with id {WorkoutId} exists", workoutId);
        }
        else
        {
            _logger.Information("No new workout id entered, keeping existing workout");
        }

        Console.Write("Введите новую дату и время в формате (гггг-мм-дд чч:мм) или нажмите Enter, чтобы пропустить: ");
        DateTime utcDateTime = DateTime.MinValue;
        string dateAndTimeInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(dateAndTimeInput))
        {
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateAndTime))
            {
                _logger.Warning("Invalid date and time format entered: {DateAndTimeInput}", dateAndTimeInput);
                Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для даты и времени");
                return;
            }

            _logger.Information("New date and time entered: {DateAndTime}", dateAndTime);

            if (dateAndTime < DateTime.Now)
            {
                _logger.Warning("Date and time in the past entered: {DateAndTime}", dateAndTime);
                Console.WriteLine("Ошибка: Введенные дата и время должны быть не в прошлом...");
                return;
            }

            utcDateTime = dateAndTime.ToUniversalTime();
            _logger.Information("Date and time converted to UTC: {UtcDateTime}", utcDateTime);
        }
        else
        {
            _logger.Information("No new date and time entered, keeping existing date and time");
        }

        Console.Write("Введите новый id клиента (если тренировка групповая, нажмите Enter): ");
        string clientIdInput = Console.ReadLine();
        Guid? clientId = null;
        if (!string.IsNullOrWhiteSpace(clientIdInput))
        {
            if (!Guid.TryParse(clientIdInput, out Guid tempClientId))
            {
                _logger.Warning("Invalid client id format entered: {ClientIdInput}", clientIdInput);
                Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id клиента");
                return;
            }

            _logger.Information("New client id entered: {ClientId}", tempClientId);

            if (!context.ClientService.CheckIfClientExists(tempClientId).Result)
            {
                _logger.Warning("Client with id {ClientId} does not exist", tempClientId);
                Console.WriteLine("Ошибка: Клиента с таким id не существует");
                return;
            }

            _logger.Information("Client with id {ClientId} exists", tempClientId);
            clientId = tempClientId;
        }
        else
        {
            _logger.Information("No new client id entered, keeping existing client or group workout");
        }

        Schedule schedule = await context.ScheduleService.GetScheduleById(scheduleId);

        schedule.IdWorkout = (workoutId == Guid.Empty) ? schedule.IdWorkout : workoutId;
        schedule.DateAndTime = (utcDateTime == DateTime.MinValue) ? schedule.DateAndTime : utcDateTime;
        schedule.IdClient = clientId;

        await context.ScheduleService.UpdateSchedule(schedule);
        Console.WriteLine("Запись успешно изменена.");
    }
}
