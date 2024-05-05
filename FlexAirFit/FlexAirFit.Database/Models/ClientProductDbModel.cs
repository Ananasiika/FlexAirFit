using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexAirFit.Database.Models;
public class ClientProductDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Required]
    [Column("id_client")]
    public Guid IdClient { get; set; }
    
    [Required]
    [Column("id_product")]
    public Guid IdProduct { get; set; }
    
    public ClientProductDbModel(Guid id, Guid idClient, Guid idProduct)
    {
        Id = id;
        IdClient = idClient;
        IdProduct = idProduct;
    }

    public ClientDbModel Client { get; set; } = null!;
    public ProductDbModel Product { get; set; } = null!;
}
