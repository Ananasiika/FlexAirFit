using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ProductCommands;

public class AddProductCommand : Command
{
    public override string? Description()
    {
        return "Добавить новый товар";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("Введите тип продукта (0 - PersonalWorkout, 1 - Membership, 2 - FoodProduct, 3 - Cloth, 4 - Solarium, 5 - Accessories):");
        ProductType type;
        if (!Enum.TryParse<ProductType>(Console.ReadLine(), out type))
        {
            Console.WriteLine("Неверный тип продукта.");
            return;
        }

        Console.WriteLine("Введите название продукта:");
        string name = Console.ReadLine();

        Console.WriteLine("Введите цену продукта:");
        int price;
        if (!int.TryParse(Console.ReadLine(), out price))
        {
            Console.WriteLine("Неверный формат цены.");
            return;
        }

        Product product = new(Guid.NewGuid(), type, name, price);
        await context.ProductService.CreateProduct(product);
        Console.WriteLine("Продукт успешно добавлен");
    }
}