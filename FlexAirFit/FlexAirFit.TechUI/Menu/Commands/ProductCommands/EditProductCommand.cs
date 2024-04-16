using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ProductCommands;

public class EditProductCommand : Command
{
    public override string? Description()
    {
        return "Изменить существующий товар";
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

        Console.WriteLine("Введите новое название продукта (или нажмите Enter, чтобы пропустить):");
        string name = Console.ReadLine();

        Console.Write("Введите новую цену (или нажмите Enter, чтобы пропустить): ");
        int price = 0;
        string priceInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(priceInput))
        {
            if (!int.TryParse(priceInput, out price))
            {
                Console.WriteLine("Ошибка: Неверный формат цены.");
                return;
            }
        }

        Product product = await context.ProductService.GetProductById(productId);

        product.Name = (string.IsNullOrEmpty(name)) ? product.Name : name;
        product.Price = (price == 0) ? product.Price : price;
        
        await context.ProductService.UpdateProduct(product);
        Console.WriteLine("Товар успешно изменен.");
    }
}