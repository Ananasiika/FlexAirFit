using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexAirFit.Database.Models;

public class ScheduleDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("workout_id")]
    [ForeignKey("Workout")]
    public Guid IdWorkout { get; set; }

    [Required]
    [Column("date_and_time")]
    public DateTime DateAndTime { get; set; }

    [Column("client_id")]
    [ForeignKey("Client")]
    public Guid? IdClient { get; set; }

    public ScheduleDbModel(Guid id, Guid idWorkout, DateTime dateAndTime, Guid? idClient)
    {
        Id = id;
        IdWorkout = idWorkout;
        DateAndTime = dateAndTime;
        IdClient = idClient;
    }

    public virtual WorkoutDbModel Workout { get; set; }

    public virtual ClientDbModel Client { get; set; }
}
