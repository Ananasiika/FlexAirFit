using FlexAirFit.Core.Enums;

namespace FlexAirFit.Core.Models;

public class Product
{
    public Guid Id { get; set; }
    public ProductType Type { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public Product(Guid id,
        ProductType type,
        string name,
        int price)
    {
        Id = id;
        Type = type;
        Name = name;
        Price = price;
    }
}