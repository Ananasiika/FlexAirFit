using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexAirFit.Database.Models;
public class ClientDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("user_id")]
    public Guid IdUser { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }

    [Required]
    [Column("gender", TypeName = "varchar(10)")]
    public string Gender { get; set; }

    [Required]
    [Column("date_of_birth", TypeName = "date")]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    [Column("membership_id")]
    public Guid IdMembership { get; set; }

    [Required]
    [Column("membership_end", TypeName = "date")]
    public DateOnly MembershipEnd { get; set; }

    [Column("remain_freezing")]
    public int? RemainFreezing { get; set; }

    [Required]
    [Column("is_freezing")]
    public bool IsFreezing { get; set; }

    public ClientDbModel(Guid id, Guid idUser, string name, string gender, DateOnly dateOfBirth, Guid idMembership, DateOnly membershipEnd, int? remainFreezing, bool isFreezing)
    {
        Id = id;
        IdUser = idUser;
        Name = name;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        IdMembership = idMembership;
        MembershipEnd = membershipEnd;
        RemainFreezing = remainFreezing;
        IsFreezing = isFreezing;
    }
    
    public List<ProductDbModel> Products { get; } = [];
    
    public List<ClientProductDbModel> ClientsProducts { get; set; } = [];

}
