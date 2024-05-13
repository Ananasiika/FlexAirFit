using FlexAirFit.Core;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ScheduleCommands;

public class AddRecordToScheduleCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<AddRecordToScheduleCommand>();

    public override string? Description()
    {
        return "Добавить запись в расписание";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing AddRecordToScheduleCommand");

        Console.Write("Введите id тренировки: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid workoutId))
        {
            _logger.Warning("Invalid workout id format entered");
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id тренировки");
            return;
        }

        _logger.Information("Workout id entered: {WorkoutId}", workoutId);

        if (!context.WorkoutService.CheckIfWorkoutExists(workoutId).Result)
        {
            _logger.Warning("Workout with id {WorkoutId} does not exist", workoutId);
            Console.WriteLine("Ошибка: Тренировки с таким id не существует");
            return;
        }

        if (context.CurrentUser.Role == UserRole.Trainer)
        {
            Workout workout = await context.WorkoutService.GetWorkoutById(workoutId);
            if (workout.IdTrainer != context.CurrentUser.Id)
            {
                _logger.Warning("Workout does not belong to the current trainer");
                Console.WriteLine("Ошибка: Тренировка не принадлежит текущему тренеру");
                return;
            }
        }

        Console.Write("Введите дату и время в формате (гггг-мм-дд чч:мм): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateAndTime))
        {
            _logger.Warning("Invalid date and time format entered");
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для даты и времени");
            return;
        }

        _logger.Information("Date and time entered: {DateAndTime}", dateAndTime);

        if (dateAndTime < DateTime.Now)
        {
            _logger.Warning("Date and time in the past entered: {DateAndTime}", dateAndTime);
            Console.WriteLine("Ошибка: Введенные дата и время должны быть не в прошлом...");
            return;
        }

        DateTime utcDateTime = dateAndTime.ToUniversalTime();
        _logger.Information("Date and time converted to UTC: {UtcDateTime}", utcDateTime);

        Guid? clientId = Guid.Empty;
        if (context.CurrentUser.Role != UserRole.Client)
        {
            Console.Write("Введите id клиента (если тренировка групповая, нажмите Enter): ");
            string clientIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(clientIdInput))
            {
                if (!Guid.TryParse(clientIdInput, out Guid tempClientId))
                {
                    _logger.Warning("Invalid client id format entered: {ClientIdInput}", clientIdInput);
                    Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id клиента");
                    return;
                }

                _logger.Information("Client id entered: {ClientId}", tempClientId);

                if (!context.ClientService.CheckIfClientExists(tempClientId).Result)
                {

                    _logger.Warning("Client with id {ClientId} does not exist", tempClientId);
                    Console.WriteLine("Ошибка: Клиента с таким id не существует");
                    return;
                }
                clientId = tempClientId;
            }
            else
            {
                _logger.Information("Group workout selected");
            }
        }
        else
        {
            clientId = context.CurrentUser.Id;
            _logger.Information("Client id set to current user: {ClientId}", clientId);
        }

        Schedule newScheduleItem = new Schedule(Guid.NewGuid(), workoutId, utcDateTime, clientId);

        try
        {
            await context.ScheduleService.CreateSchedule(newScheduleItem);
            Console.WriteLine("Запись успешно добавлена в расписание.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while adding schedule item: {@ScheduleItem}", newScheduleItem);
            Console.WriteLine($"Произошла ошибка при добавлении записи в расписание.");
            return;
        }
    }
}
