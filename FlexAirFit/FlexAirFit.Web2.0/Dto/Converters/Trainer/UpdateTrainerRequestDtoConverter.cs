using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class UpdateTrainerRequestDtoConverter
{
    public static Trainer? ToCore(UpdateTrainerRequestDto dto, Guid id)
    {
        return new Trainer(id, dto.Name, dto.Gender, dto.Specialization, dto.Experience, dto.Rating);
    }
}