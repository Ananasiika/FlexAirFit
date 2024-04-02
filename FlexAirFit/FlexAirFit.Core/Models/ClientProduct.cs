namespace FlexAirFit.Core.Models;

public class ClientProduct
{
    public Guid IdClient { get; set; }
    public Guid IdProduct { get; set; }

    public ClientProduct(Guid idClient,
        Guid idProduct)
    {
        IdClient = idClient;
        IdProduct = idProduct;
    }
}