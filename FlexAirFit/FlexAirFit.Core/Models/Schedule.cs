namespace FlexAirFit.Core.Models;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid IdWorkout { get; set; }
    public DateTime DateAndTime { get; set; }
    public Guid? IdClient { get; set; }

    public Schedule(Guid id,
        Guid idWorkout,
        DateTime dateAndTime,
        Guid idClient)
    {
        Id = id;
        IdWorkout = idWorkout;
        DateAndTime = dateAndTime;
        IdClient = idClient;
    }
}