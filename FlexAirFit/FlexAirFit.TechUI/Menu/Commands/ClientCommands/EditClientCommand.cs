using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ClientCommands;

public class EditClientCommand : Command
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
            
        DateOnly dateOfBirth = DateOnly.MinValue;
        Console.WriteLine("Введите вашу новую дату рождения в формате YYYY-MM-DD (или нажмите Enter, чтобы пропустить):");
        string dateOfBirthInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(dateOfBirthInput))
        {
            if (!DateOnly.TryParse(dateOfBirthInput, out dateOfBirth))
            {
                Console.WriteLine("Ошибка: Неверный формат даты рождения");
                return;
            }
        }
        
        Client client = await context.ClientService.GetClientById(context.CurrentUser.Id);

        client.DateOfBirth = (dateOfBirth == DateOnly.MinValue) ? client.DateOfBirth : dateOfBirth;
        client.Gender = (string.IsNullOrEmpty(gender)) ? client.Gender : gender;
        client.Name = (string.IsNullOrEmpty(name)) ? client.Name : name;

        try
        {
            await context.ClientService.UpdateClient(client);
        }
        catch (Exception e)
        {
            Console.WriteLine("При изменении произошла ошибка", e);
            return;
        }
        
        Console.WriteLine("Личная информация успешно изменена.");
    }
}