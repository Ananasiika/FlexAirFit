using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;

namespace FlexAirFit.Application.Services;

public class MembershipService(IMembershipRepository membershipRepository) : IMembershipService
{
    private readonly IMembershipRepository _membershipRepository = membershipRepository;

    public async Task CreateMembership(Membership membership)
    {
        if (await _membershipRepository.GetMembershipByIdAsync(membership.Id) is not null)
        {
            throw new MembershipExistsException(membership.Id);
        }
        await _membershipRepository.AddMembershipAsync(membership);
    }

    public async Task<Membership> UpdateMembership(Membership membership)
    {
        if (await _membershipRepository.GetMembershipByIdAsync(membership.Id) is null)
        {
            throw new MembershipNotFoundException(membership.Id);
        }
        return await _membershipRepository.UpdateMembershipAsync(membership);
    }

    public async Task DeleteMembership(Guid idMembership)
    {
        if (await _membershipRepository.GetMembershipByIdAsync(idMembership) is null)
        {
            throw new MembershipNotFoundException(idMembership);
        }
        await _membershipRepository.DeleteMembershipAsync(idMembership);
    }
    
    public async Task<Membership> GetMembershipById(Guid idMembership)
    {
        return await _membershipRepository.GetMembershipByIdAsync(idMembership) ?? throw new MembershipNotFoundException(idMembership);
    }
    
    public async Task<List<Membership>> GetMemberships(int? limit, int? offset)
    {
        return await _membershipRepository.GetMembershipsAsync(limit, offset);
    }
    
    public async Task<bool> CheckIfMembershipExists(Guid idMembership)
    {
        if (await _membershipRepository.GetMembershipByIdAsync(idMembership) is null)
        {
            return false;
        }
        return true;
    }
}