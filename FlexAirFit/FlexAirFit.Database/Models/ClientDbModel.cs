using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace FlexAirFit.Database.Models;
public class ClientDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }

    [Required]
    [Column("gender", TypeName = "varchar(10)")]
    public string Gender { get; set; }

    [Required]
    [Column("date_of_birth", TypeName = "date")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Column("membership_id")]
    public Guid IdMembership { get; set; }

    [Required]
    [Column("membership_end", TypeName = "date")]
    public DateTime MembershipEnd { get; set; }

    [Column("remain_freezing")]
    public int? RemainFreezing { get; set; }

    [Required]
    [Column("is_membership_active")]
    public bool IsMembershipActive { get; }
    
    [Required]
    [Column("freezing_intervals")]
    public JsonDocument? FreezingIntervals { get; set; }
    
    public ClientDbModel(Guid id, string name, string gender, DateTime dateOfBirth, Guid idMembership, DateTime membershipEnd, int? remainFreezing)
    {
        Id = id;
        Name = name;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        IdMembership = idMembership;
        MembershipEnd = membershipEnd;
        RemainFreezing = remainFreezing;
    }
    
    public ClientDbModel(Guid id, string name, string gender, DateTime dateOfBirth, Guid idMembership, DateTime membershipEnd, int? remainFreezing, JsonDocument? freezingIntervals)
        : this(id, name, gender, dateOfBirth, idMembership, membershipEnd, remainFreezing)
    {
        FreezingIntervals = freezingIntervals;
    }
    
    public List<ProductDbModel> Products { get; } = [];
    
    public List<ClientProductDbModel> ClientsProducts { get; set; } = [];

}
