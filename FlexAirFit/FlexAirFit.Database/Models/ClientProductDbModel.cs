using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexAirFit.Database.Models;
public class ClientProductDbModel(Guid idClient, Guid idProduct)
{
    public Guid IdClient { get; set; } = idClient;
    public Guid IdProduct { get; set; } = idProduct;

    public ClientDbModel Client { get; set; } = null!;
    public ProductDbModel Product { get; set; } = null!;
}
