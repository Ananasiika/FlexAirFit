namespace FlexAirFit.Web.Models;
public class WorkoutFilterModel
{
    public string? Name { get; set; }
    public string? NameTrainer { get; set; }
    public TimeSpan? MinDuration { get; set; }
    public TimeSpan? MaxDuration { get; set; }
    public int? MinLevel { get; set; }
    public int? MaxLevel { get; set; }
    public int PageNumber { get; set; } = 1;
}

public class WorkoutFilterModelResult
{
    public IEnumerable<WorkoutModel> Workouts { get; set; }
    public WorkoutFilterModel Filter { get; set; }
}