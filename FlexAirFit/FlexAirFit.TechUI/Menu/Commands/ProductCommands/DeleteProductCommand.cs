using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ProductCommands;

public class DeleteProductCommand : Command
{
    public override string? Description()
    {
        return "Удалить существующий товар";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите id товара: ");
        string productIdInput = Console.ReadLine();
        if (!Guid.TryParse(productIdInput, out Guid productId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id товара");
            return;
        }

        if (!context.ProductService.CheckIfProductExists(productId).Result)
        {
            Console.WriteLine("Ошибка: Товара с таким id не существует");
            return;
        }

        try
        {
            await context.ProductService.DeleteProduct(productId);
        }
        catch (Exception e)
        {
            Console.WriteLine("При удалении произошла ошибка", e);
            return;
        }
        Console.WriteLine("Товар успешно удален");
    }
}