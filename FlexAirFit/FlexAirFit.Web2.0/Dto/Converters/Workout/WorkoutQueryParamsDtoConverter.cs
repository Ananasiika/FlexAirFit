using FlexAirFit.Core.Filters;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class WorkoutQueryParamsDtoConverter
{
    public static FilterWorkout ToCore(WorkoutQueryParamsDto? dto)
    {
        return dto is null ? new FilterWorkout(null, null, null, null, null, null) : 
            new FilterWorkout(dto.NameWorkout, dto.NameTrainer, dto.MinDuration, dto.MaxDuration, dto.MinLevel, dto.MaxLevel);
    }
}