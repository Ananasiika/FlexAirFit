using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class UpdateMembershipRequestDtoConverter
{
    public static Membership? ToCore(UpdateMembershipRequestDto dto, Guid id)
    {
        return new Membership(id, dto.Name, dto.Duration, dto.Price, dto.Freezing);
    }
}