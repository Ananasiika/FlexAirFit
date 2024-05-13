using FlexAirFit.TechUI.BaseMenu;
using Serilog;

namespace FlexAirFit.TechUI.Commands.ProductCommands;

public class DeleteProductCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<DeleteProductCommand>();

    public override string? Description()
    {
        return "Удалить существующий товар";
    }

    public override async Task Execute(Context context)
    {
        _logger.Information("Executing DeleteProductCommand");

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

        try
        {
            await context.ProductService.DeleteProduct(productId);
            Console.WriteLine("Товар успешно удален");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while deleting product with id {ProductId}", productId);
            Console.WriteLine("При удалении произошла ошибка", ex);
        }
    }
}