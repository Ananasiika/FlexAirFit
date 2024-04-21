using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexAirFit.Database.Models;

public class AdminDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }

    [Required]
    [Column("date_of_birth", TypeName = "datetime")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Column("gender", TypeName = "varchar(10)")]
    public string Gender { get; set; }

    public AdminDbModel(Guid id, string name, DateTime dateOfBirth, string gender)
    {
        Id = id;
        Name = name;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }
}
