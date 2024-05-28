using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexAirFit.Database.Models;

public class MembershipDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }

    [Required]
    [Column("duration", TypeName = "interval")]
    public TimeSpan Duration { get; set; }

    [Required]
    [Column("price")]
    public int Price { get; set; }

    [Required]
    [Column("freezing")]
    public int Freezing { get; set; }

    public MembershipDbModel(Guid id, string name, TimeSpan duration, int price, int freezing)
    {
        Id = id;
        Name = name;
        Duration = duration;
        Price = price;
        Freezing = freezing;
    }
}
