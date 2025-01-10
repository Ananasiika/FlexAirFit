using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class CreateWorkoutRequestDtoConverter
{
    public static Workout? ToCore(CreateWorkoutRequestDto dto)
    {
        try
        {
            return new Workout(Guid.NewGuid(), dto.Name, dto.Description, dto.IdTrainer, dto.Duration, dto.Level);
        }
        catch (Exception e)
        {
            throw new CreateWorkoutRequestException();
        }
    }
}