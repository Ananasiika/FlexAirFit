using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ProductCommands;

public class ViewProductsCommand : Command
{
    public override string? Description()
    {
        return "Просмотр товаров";
    }

    public override async Task Execute(Context context)
    {
        var products = await context.ProductService.GetProducts(null, null);

        if (products.Count == 0)
        {
            Console.WriteLine("Нет товаров.");
        }
        else
        {
            Console.WriteLine("Список товаров:");

            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}");
                Console.WriteLine($"Тип: {product.Type}");
                Console.WriteLine($"Название: {product.Name}");
                Console.WriteLine($"Цена: {product.Price}");
                Console.WriteLine();
            }
        }
    }

}