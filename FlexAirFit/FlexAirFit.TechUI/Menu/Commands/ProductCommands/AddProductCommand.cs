using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ProductCommands;

public class AddProductCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<AddProductCommand>();

    public override string? Description()
    {
        return "Добавить новый товар";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing AddProductCommand");

        Console.WriteLine("Введите тип продукта (0 - PersonalWorkout, 1 - Membership, 2 - FoodProduct, 3 - Cloth, 4 - Solarium, 5 - Accessories):");
        ProductType type;
        if (!Enum.TryParse<ProductType>(Console.ReadLine(), out type))
        {
            _logger.Warning("Invalid product type entered");
            Console.WriteLine("Неверный тип продукта.");
            return;
        }

        _logger.Information("Product type entered: {ProductType}", type);

        Console.WriteLine("Введите название продукта:");
        string name = Console.ReadLine();
        _logger.Information("Product name entered: {ProductName}", name);

        Console.WriteLine("Введите цену продукта:");
        int price;
        if (!int.TryParse(Console.ReadLine(), out price))
        {
            _logger.Warning("Invalid product price format entered");
            Console.WriteLine("Неверный формат цены.");
            return;
        }

        _logger.Information("Product price entered: {ProductPrice}", price);

        Guid productId = Guid.NewGuid();
        Product product = new(productId, type, name, price);

        try
        {
            await context.ProductService.CreateProduct(product);
            Console.WriteLine("Продукт успешно добавлен");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while creating product with id {ProductId}", productId);
            throw;
        }
    }
}
