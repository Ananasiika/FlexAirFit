using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class CreateMembershipRequestDtoConverter
{
    public static Membership? ToCore(CreateMembershipRequestDto dto)
    {
        try
        {
            return new Membership(Guid.NewGuid(), dto.Name, dto.Duration, dto.Price, dto.Freezing);
        }
        catch (Exception e)
        {
            throw new CreateMembershipRequestException();
        }
    }
}