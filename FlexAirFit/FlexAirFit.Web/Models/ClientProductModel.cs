namespace FlexAirFit.Web.Models;

public class ClientProductModel
{
    public Guid IdProduct { get; set; }
    public Guid IdClient { get; set; }
    public int Price { get; set; }
    public string Name { get; set; }
    public bool UseBonus { get; set; }
    public int Cost { get; set; }
}

public class BuyProductModel
{
    public string Name { get; set; }
    public int Price { get; set; }
    public Guid IdProduct { get; set; }
    public bool UseBonus { get; set; }
}
