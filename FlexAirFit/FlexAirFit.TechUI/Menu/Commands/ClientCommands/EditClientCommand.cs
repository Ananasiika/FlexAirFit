using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class EditClientCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<EditClientCommand>();

    public override string? Description()
    {
        return "Изменить личную информацию";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing EditClientCommand");

        Console.WriteLine("Введите ваше новое имя (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();
        _logger.Information("Entered new name: {Name}", name);

        Console.WriteLine("Выберите ваш новый пол (male, female) (или нажмите Enter, чтобы пропустить):");
        string gender = Console.ReadLine();
        _logger.Information("Entered new gender: {Gender}", gender);

        DateTime dateOfBirth = DateTime.MinValue;
        Console.WriteLine("Введите вашу новую дату рождения в формате YYYY-MM-DD (или нажмите Enter, чтобы пропустить):");
        string dateOfBirthInput = Console.ReadLine();
        _logger.Information("Entered new date of birth: {DateOfBirthInput}", dateOfBirthInput);
        if (!string.IsNullOrEmpty(dateOfBirthInput))
        {
            if (!DateTime.TryParse(dateOfBirthInput, out dateOfBirth))
            {
                _logger.Error("Invalid date of birth format");
                Console.WriteLine("Ошибка: Неверный формат даты рождения");
                return;
            }
        }

        Client client = await context.ClientService.GetClientById(context.CurrentUser.Id);

        client.DateOfBirth = (dateOfBirth == DateTime.MinValue) ? client.DateOfBirth : dateOfBirth;
        client.Gender = (string.IsNullOrEmpty(gender)) ? client.Gender : gender;
        client.Name = (string.IsNullOrEmpty(name)) ? client.Name : name;

        try
        {
            await context.ClientService.UpdateClient(client);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error updating client");
            Console.WriteLine("При изменении произошла ошибка", e);
            return;
        }

        Console.WriteLine("Личная информация успешно изменена.");
    }
}
