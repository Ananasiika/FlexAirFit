using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class UpdateScheduleRequestDtoConverter
{
    public static Schedule? ToCore(UpdateScheduleRequestDto dto, Guid id)
    {
        return new Schedule(id, dto.IdWorkout, dto.DateAndTime, dto.IdClient);
    }
}