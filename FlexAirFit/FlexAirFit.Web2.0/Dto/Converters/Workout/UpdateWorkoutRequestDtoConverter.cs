using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class UpdateWorkoutRequestDtoConverter
{
    public static Workout? ToCore(UpdateWorkoutRequestDto dto, Guid id)
    {
        return new Workout(id, dto.Name, dto.Description, dto.IdTrainer, dto.Duration, dto.Level);
    }
}