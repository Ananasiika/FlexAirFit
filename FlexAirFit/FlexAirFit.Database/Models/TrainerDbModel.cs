using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexAirFit.Database.Models;

public class TrainerDbModel
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
    [Column("specialization", TypeName = "varchar(50)")]
    public string Specialization { get; set; }

    [Required]
    [Column("experience")]
    public int Experience { get; set; }

    [Required]
    [Column("rating")]
    public int Rating { get; set; }

    public TrainerDbModel(Guid id, string name, string gender, string specialization, int experience, int rating)
    {
        Id = id;
        Name = name;
        Gender = gender;
        Specialization = specialization;
        Experience = experience;
        Rating = rating;
    }
}
