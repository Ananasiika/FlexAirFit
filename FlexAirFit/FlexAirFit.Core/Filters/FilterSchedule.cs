namespace FlexAirFit.Core.Filters;

public class FilterSchedule
{
    public string? NameWorkout { get; init; }
    public DateTime? MinDateAndTime { get; init; }
    public DateTime? MaxDateAndTime { get; init; }

    public FilterSchedule(string? nameWorkout,
        DateTime? minDateAndTime,
        DateTime? maxDateAndTime)
    {
        NameWorkout = nameWorkout;
        MinDateAndTime = minDateAndTime;
        MaxDateAndTime = maxDateAndTime;
    }
}