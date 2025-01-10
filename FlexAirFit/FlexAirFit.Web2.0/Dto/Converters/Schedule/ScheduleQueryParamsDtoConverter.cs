using FlexAirFit.Core.Filters;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class ScheduleQueryParamsDtoConverter
{
    public static FilterSchedule ToCore(ScheduleQueryParamsDto? dto)
    {
        return dto is null ? new FilterSchedule(null, null, null, null, null, null) : 
            new FilterSchedule(dto.NameWorkout, dto.MinDateAndTime, dto.MaxDateAndTime, dto.WorkoutType, dto.ClientId, dto.TrainerId);
    }
}