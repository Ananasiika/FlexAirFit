using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Database.Models;
public class ProductDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("type", TypeName = "varchar(50)")]
    public ProductType Type { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }

    [Required]
    [Column("price")]
    public int Price { get; set; }

    public ProductDbModel(Guid id, ProductType type, string name, int price)
    {
        Id = id;
        Type = type;
        Name = name;
        Price = price;
    }
    
    public List<ClientDbModel> Clients { get; } = [];
    public List<ClientProductDbModel> ClientsProducts { get; } = [];
}