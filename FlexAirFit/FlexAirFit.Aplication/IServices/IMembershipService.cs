using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IServices;

public interface IMembershipService
{
    Task CreateMembership(Membership membership);
    Task<Membership> UpdateMembership(Guid membershipId, string? name, TimeSpan? duration, int? price, int? freezing);
    Task DeleteMembership(Guid id);
    Task<Membership> GetMembershipById(Guid id);
    Task<List<Membership>> GetMemberships(int? limit, int? offset);
    Task<bool> CheckIfMembershipExists(Guid membershipId);
}
