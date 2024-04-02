using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IRepositories;

public interface IMembershipRepository
{
    Task AddMembershipAsync(Membership membership);
    Task<Membership> UpdateMembershipAsync(Membership membership);
    Task DeleteMembershipAsync(Guid id);
    Task<Membership> GetMembershipByIdAsync(Guid id);
    Task<List<Membership>> GetMembershipsAsync(int? limit, int? offset);
}