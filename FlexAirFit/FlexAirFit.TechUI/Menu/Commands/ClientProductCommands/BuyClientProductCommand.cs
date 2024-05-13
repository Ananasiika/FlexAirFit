using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ClientProductCommands;

public class BuyClientProductCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<BuyClientProductCommand>();

    public override string? Description()
    {
        return "Купить товар";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing BuyClientProductCommand");

        Console.Write("Введите id товара: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid productId))
        {
            _logger.Warning("Invalid product id format entered");
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id товара");
            return;
        }

        _logger.Information("Product id entered: {ProductId}", productId);

        Console.WriteLine("Хотите списывать бонусы? (true/false)");
        if (!bool.TryParse(Console.ReadLine(), out bool writeOff))
        {
            _logger.Warning("Invalid input for writeOff option");
            Console.WriteLine("Ошибка: Введенное значение не true или false");
            return;
        }

        _logger.Information("WriteOff option: {WriteOff}", writeOff);

        ClientProduct clientProduct = new(context.CurrentUser.Id, productId);
        int cost;
        try
        {
            cost = await context.ClientProductService.AddClientProductAndReturnCost(clientProduct, writeOff);
            _logger.Information("Product cost: {Cost}", cost);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while adding client product");

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
