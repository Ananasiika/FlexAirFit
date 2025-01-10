using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public static class MembershipDtoConverter
{
    public static MembershipDto? ToDto(this Membership? membership)
    {
        return membership is null ? null : new MembershipDto(membership.Id, membership.Name, membership.Duration, membership.Price, membership.Freezing);
    }
    
    public static Membership? ToCore(this MembershipDto? membership)
    {
        return membership is null ? null : new Membership(membership.Id, membership.Name, membership.Duration, membership.Price, membership.Freezing);
    }
}