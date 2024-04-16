namespace FlexAirFit.Core.Filters;

public class FilterSchedule
{
    public string? NameWorkout { get; init; }
    public DateTime? MinDateAndTime { get; init; }
    public DateTime? MaxDateAndTime { get; init; }
    public Guid? ClientId { get; init; }
    public Guid? TrainerId { get; init; }

    public FilterSchedule(string? nameWorkout,
        DateTime? minDateAndTime,
        DateTime? maxDateAndTime,
        Guid? clientId,
        Guid? trainerId)
    {
        NameWorkout = nameWorkout;
        MinDateAndTime = minDateAndTime;
        MaxDateAndTime = maxDateAndTime;
        ClientId = clientId;
        TrainerId = trainerId;
    }
}