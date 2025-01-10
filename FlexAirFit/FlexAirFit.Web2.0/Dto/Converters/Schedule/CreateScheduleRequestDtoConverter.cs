using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class CreateScheduleRequestDtoConverter
{
    public static Schedule? ToCore(CreateScheduleRequestDto dto)
    {
        try
        {
            return new Schedule(Guid.NewGuid(), dto.IdWorkout, dto.DateAndTime, dto.IdClient);
        }
        catch (Exception e)
        {
            throw new CreateScheduleRequestException();
        }
    }
}