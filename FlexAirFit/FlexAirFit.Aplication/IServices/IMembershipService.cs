using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IServices;

public interface IMembershipService
{
    Task CreateMembership(Membership membership);
    Task<Membership> UpdateMembership(Membership membership);
    Task DeleteMembership(Guid id);
    Task<Membership> GetMembershipById(Guid id);
    Task<List<Membership>> GetMemberships(int? limit, int? offset);
    Task<bool> CheckIfMembershipExists(Guid membershipId);
}