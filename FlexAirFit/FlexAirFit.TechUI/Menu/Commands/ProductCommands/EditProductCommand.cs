using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ProductCommands;

public class EditProductCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<EditProductCommand>();

    public override string? Description()
    {
        return "Изменить существующий товар";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing EditProductCommand");

        Console.WriteLine("Введите id товара: ");
        string productIdInput = Console.ReadLine();
        if (!Guid.TryParse(productIdInput, out Guid productId))
        {
            _logger.Warning("Invalid product id format entered");
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id товара");
            return;
        }

        _logger.Information("Product id entered: {ProductId}", productId);

        if (!context.ProductService.CheckIfProductExists(productId).Result)
        {
            _logger.Warning("Product with id {ProductId} does not exist", productId);
            Console.WriteLine("Ошибка: Товара с таким id не существует");
            return;
        }

        _logger.Information("Product with id {ProductId} exists, proceeding to edit", productId);

        Console.WriteLine("Введите новое название продукта (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();
        _logger.Information("New product name entered: {ProductName}", name ?? "<empty>");

        Console.Write("Введите новую цену (или нажмите Enter, чтобы пропустить): ");
        int price = 0;
        string priceInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(priceInput))
        {
            if (!int.TryParse(priceInput, out price))
            {
                _logger.Warning("Invalid price format entered: {PriceInput}", priceInput);
                Console.WriteLine("Ошибка: Неверный формат цены.");
                return;
            }
        }

        _logger.Information("New product price entered: {ProductPrice}", price);

        Product product = await context.ProductService.GetProductById(productId);

        product.Name = (string.IsNullOrEmpty(name)) ? product.Name : name;
        product.Price = (price == 0) ? product.Price : price;

        try
        {
            await context.ProductService.UpdateProduct(product);
            Console.WriteLine("Товар успешно изменен.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while updating product with id {ProductId}", productId);
            throw;
        }
    }
}
