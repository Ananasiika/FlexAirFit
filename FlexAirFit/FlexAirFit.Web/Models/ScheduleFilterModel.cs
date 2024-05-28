using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web.Models;
public class ScheduleFilterModel
{
    public string? NameWorkout { get; set; }
    public DateTime? MinDateAndTime { get; set; }
    public DateTime? MaxDateAndTime { get; set; }
    public WorkoutType? WorkoutType { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? TrainerId { get; set; }
    public string? NameTrainer { get; set; }
    public string? NameClient { get; set; }
    public int PageNumber { get; set; } = 1;
}

public class ScheduleFilterModelResult
{
    public IEnumerable<ScheduleModel> Schedules { get; set; }
    public ScheduleFilterModel Filter { get; set; }
    public IList<WorkoutType> WorkoutTypes { get; set; }
}