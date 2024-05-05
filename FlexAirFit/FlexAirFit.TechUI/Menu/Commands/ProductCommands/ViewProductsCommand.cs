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
        var products = await context.ProductService.GetProducts(10, null);
        if (products.Count == 0)
        {
            Console.WriteLine("Нет товаров.");
            return;
        }
        
        Console.WriteLine("Список товаров:");
        int page = 1;
        while (products.Count != 0)
        {
            if (page != 1)
            {
                Console.WriteLine("Введите 0, чтобы закончить или Enter, чтобы продолжить:");
                string nextInput = Console.ReadLine();
                if (!int.TryParse(nextInput, out int next))
                {
                    next = 1;
                }

                if (next == 0)
                {
                    return;
                }
            }
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}");
                Console.WriteLine($"Тип: {product.Type}");
                Console.WriteLine($"Название: {product.Name}");
                Console.WriteLine($"Цена: {product.Price}");
                Console.WriteLine();
            }
            products = await context.ProductService.GetProducts(10, 10 * page++);
        }
    }

}