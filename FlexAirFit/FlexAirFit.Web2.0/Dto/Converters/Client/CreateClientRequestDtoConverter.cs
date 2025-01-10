using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class CreateClientRequestDtoConverter
{
    public static Client? ToCore(Guid id, CreateClientRequestDto dto, TimeSpan duration, int freezing)
    {
        return new Client(id, dto.Name, dto.Gender, dto.DateOfBirth, dto.IdMembership, DateTime.Today.Add(duration), freezing, null);
    }
}