namespace FlexAirFit.Core.Models;

public class Membership
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TimeSpan Duration { get; set; }
    public int Price { get; set; }
    public int Freezing { get; set; }

    public Membership(Guid id,
        string name,
        TimeSpan duration,
        int price,
        int freezing)
    {
        Id = id;
        Name = name;
        Duration = duration;
        Price = price;
        Freezing = freezing;
    }
}