namespace FlexAirFit.Core.Models;

public class Workout
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid IdTrainer { get; set; }
    public TimeSpan Duration { get; set; }
    public int Level { get; set; }

    public Workout(Guid id,
        string name,
        string description,
        Guid idTrainer,
        TimeSpan duration,
        int level)
    {
        Id = id;
        Name = name;
        Description = description;
        IdTrainer = idTrainer;
        Duration = duration;
        Level = level;
    }
}