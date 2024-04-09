namespace FlexAirFit.Core.Models;

public class Bonus
{
    public Guid Id { get; set; }
    public Guid IdClient { get; set; }
    public int? Count { get; set; }

    public Bonus(Guid id,
        Guid idClient,
        int? count)
    {
        Id = id;
        IdClient = idClient;
        Count = count;
    }
}