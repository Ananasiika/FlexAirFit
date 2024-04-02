using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlexAirFit.Core.Models;

namespace FlexAirFit.Database.Models;

public class BonusDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [ForeignKey("Client")]
    [Column("client_id")]
    public Guid IdClient { get; set; }
    
    [Required]
    [Column("Count")]
    public int Count { get; set; }
    
    public BonusDbModel(Guid id, Guid idClient, int count)
    {
        Id = id;
        IdClient = idClient;
        Count = count;
    }
    public ClientDbModel Client { get; set; }
}
