using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ClientProductCommands;

public class BuyClientProductCommand : Command
{
    public override string? Description()
    {
        return "Купить товар";
    }

    public override async Task Execute(Context context)
    {
        Console.Write("Введите id товара: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid productId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id товара");
            return;
        }
        Console.WriteLine("Хотите списывать бонусы? (true/false)");
        if (!bool.TryParse(Console.ReadLine(), out bool writeOff))
        {
            Console.WriteLine("Ошибка: Введенное значение не true или false");
            return;
        }

        ClientProduct clientProduct = new(context.CurrentUser.Id, productId);
        int cost;
        try
        {
            cost = await context.ClientProductService.AddClientProductAndReturnCost(clientProduct, writeOff);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
            else
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            throw;
        }
        Console.WriteLine($"К оплате {cost} рублей");
    }
}