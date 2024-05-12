using FlexAirFit.Core.Enums;

namespace FlexAirFit.Core.Filters;

public class FilterSchedule
{
    public string? NameWorkout { get; init; }
    public DateTime? MinDateAndTime { get; init; }
    public DateTime? MaxDateAndTime { get; init; }
    public WorkoutType? WorkoutType { get; init; }
    public Guid? ClientId { get; init; }
    public Guid? TrainerId { get; init; }

    public FilterSchedule(string? nameWorkout,
        DateTime? minDateAndTime,
        DateTime? maxDateAndTime,
        WorkoutType? workoutType,
        Guid? clientId,
        Guid? trainerId)
    {
        NameWorkout = nameWorkout;
        MinDateAndTime = minDateAndTime;
        MaxDateAndTime = maxDateAndTime;
        WorkoutType = workoutType;
        ClientId = clientId;
        TrainerId = trainerId;
    }
}