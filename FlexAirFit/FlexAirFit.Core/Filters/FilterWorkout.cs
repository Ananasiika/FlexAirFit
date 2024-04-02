namespace FlexAirFit.Core.Filters;

public class FilterWorkout
{
    public string? Name { get; init; }
    public string? NameTrainer { get; init; }
    public TimeSpan? MinDuration { get; init; }
    public TimeSpan? MaxDuration { get; init; }
    public int? MinLevel { get; init; }
    public int? MaxLevel { get; init; }
    
    public FilterWorkout(string? name, 
        string? nameTrainer, 
        TimeSpan? minDuration, 
        TimeSpan? maxDuration, 
        int? minLevel, 
        int? maxLevel)
    {
        Name = name;
        NameTrainer = nameTrainer;
        MinDuration = minDuration;
        MaxDuration = maxDuration;
        MinLevel = minLevel;
        MaxLevel = maxLevel;
    }
}