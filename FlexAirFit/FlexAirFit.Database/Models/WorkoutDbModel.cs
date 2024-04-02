using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexAirFit.Database.Models
{
    public class WorkoutDbModel
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Required]
        [Column("description", TypeName = "varchar(200)")]
        public string Description { get; set; }

        [Required]
        [Column("trainer_id")]
        [ForeignKey("Trainer")]
        public Guid IdTrainer { get; set; }

        [Required]
        [Column("duration")]
        public TimeSpan Duration { get; set; }

        [Required]
        [Column("level")]
        public int Level { get; set; }

        public WorkoutDbModel(Guid id, string name, string description, Guid idTrainer, TimeSpan duration, int level)
        {
            Id = id;
            Name = name;
            Description = description;
            IdTrainer = idTrainer;
            Duration = duration;
            Level = level;
        }

        public virtual TrainerDbModel Trainer { get; set; }
    }
}