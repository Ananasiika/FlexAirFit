using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public static class ScheduleDtoConverter
{
    public static ScheduleDto? ToDto(this Schedule? schedule)
    {
        return schedule is null ? null : new ScheduleDto(schedule.Id, schedule.IdWorkout, schedule.DateAndTime, schedule.IdClient);
    }
    
    public static Schedule? ToCore(this ScheduleDto? schedule)
    {
        return schedule is null ? null : new Schedule(schedule.Id,schedule.IdWorkout, schedule.DateAndTime, schedule.IdClient);
    }
}