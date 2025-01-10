using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class CreateTrainerRequestDtoConverter
{
    public static Trainer? ToCore(Guid id, CreateTrainerRequestDto dto)
    {
        try
        {
            return new Trainer(id, dto.Name, dto.Gender, dto.Specialization, dto.Experience, dto.Rating);
        }
        catch (Exception e)
        {
            throw new CreateTrainerRequestException();
        }
    }
}