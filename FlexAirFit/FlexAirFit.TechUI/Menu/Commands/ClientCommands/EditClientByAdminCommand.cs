using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class EditClientByAdminCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<EditClientByAdminCommand>();

    public override string? Description()
    {
        return "Изменить личную информацию клиента";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing EditClientByAdminCommand");

        Console.Write("Введите id клиента: ");
        string clientIdInput = Console.ReadLine();
        _logger.Information($"Entered client id: {clientIdInput}");

        if (!Guid.TryParse(clientIdInput, out Guid clientId))
        {
            _logger.Error("Invalid client id format");
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id клиента");
            return;
        }

        if (!context.ClientService.CheckIfClientExists(clientId).Result)
        {
            _logger.Error($"Client with id {clientId} not found");
            Console.WriteLine("Ошибка: Клиента с таким id не существует");
            return;
        }

        Console.WriteLine("Введите новое имя (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();
        _logger.Information($"Entered new name: {name}");

        Console.WriteLine("Выберите новый пол (male, female) (или нажмите Enter, чтобы пропустить):");
        string gender = Console.ReadLine();
        _logger.Information($"Entered new gender: {gender}");

        DateTime dateOfBirth = DateTime.MinValue;
        Console.WriteLine("Введите новую дату рождения в формате YYYY-MM-DD (или нажмите Enter, чтобы пропустить):");
        string dateOfBirthInput = Console.ReadLine();
        _logger.Information($"Entered new date of birth: {dateOfBirthInput}");

        if (!string.IsNullOrEmpty(dateOfBirthInput))
        {
            if (!DateTime.TryParse(dateOfBirthInput, out dateOfBirth))
            {
                _logger.Error("Invalid date of birth format");
                Console.WriteLine("Ошибка: Неверный формат даты рождения");
                return;
            }
        }

        Client client = await context.ClientService.GetClientById(clientId);

        client.DateOfBirth = (dateOfBirth == DateTime.MinValue) ? client.DateOfBirth : dateOfBirth;
        client.Gender = (string.IsNullOrEmpty(gender)) ? client.Gender : gender;
        client.Name = (string.IsNullOrEmpty(name)) ? client.Name : name;

        try
        {
            await context.ClientService.UpdateClient(client);
        }
        catch (Exception e)
        {
            _logger.Error($"Error updating client {clientId} information: {e.Message}");
            Console.WriteLine("При изменении произошла ошибка", e);
            return;
        }

        Console.WriteLine("Личная информация успешно изменена.");
    }
}
