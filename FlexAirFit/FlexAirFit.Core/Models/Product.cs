namespace FlexAirFit.Core.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public Product(Guid id,
        string type,
        string name,
        int price)
    {
        Id = id;
        Type = type;
        Name = name;
        Price = price;
    }
}