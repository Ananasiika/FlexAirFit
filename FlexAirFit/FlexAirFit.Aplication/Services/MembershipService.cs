using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Serilog;

namespace FlexAirFit.Application.Services;

public class MembershipService(IMembershipRepository membershipRepository) : IMembershipService
{
    private readonly IMembershipRepository _membershipRepository = membershipRepository;
    private readonly ILogger _logger = Log.ForContext<MembershipService>();

    public async Task CreateMembership(Membership membership)
    {
        if (await _membershipRepository.GetMembershipByIdAsync(membership.Id) is not null)
        {
            _logger.Warning($"Membership with id {membership.Id} already exists. Skipping creation.");
            throw new MembershipExistsException(membership.Id);
        }
        await _membershipRepository.AddMembershipAsync(membership);
    }

    public async Task<Membership> UpdateMembership(Membership membership)
    {
        if (await _membershipRepository.GetMembershipByIdAsync(membership.Id) is null)
        {
            _logger.Warning($"Membership with id {membership.Id} not found. Skipping update.");
            throw new MembershipNotFoundException(membership.Id);
        }
        return await _membershipRepository.UpdateMembershipAsync(membership);
    }

    public async Task DeleteMembership(Guid idMembership)
    {
        if (await _membershipRepository.GetMembershipByIdAsync(idMembership) is null)
        {
            _logger.Warning($"Membership with id {idMembership} not found. Skipping deletion.");
            throw new MembershipNotFoundException(idMembership);
        }
        await _membershipRepository.DeleteMembershipAsync(idMembership);
    }
    
    public async Task<Membership> GetMembershipById(Guid idMembership)
    {
        var membership = await _membershipRepository.GetMembershipByIdAsync(idMembership);
        if (membership is null)
        {
            _logger.Error($"Membership with id {idMembership} not found.");
            throw new MembershipNotFoundException(idMembership);
        }

        _logger.Information($"Membership with ID {idMembership} was successfully retrieved.");
        return membership;
    }
    
    public async Task<List<Membership>> GetMemberships(int? limit, int? offset)
    {
        return await _membershipRepository.GetMembershipsAsync(limit, offset);
    }
    
    public async Task<bool> CheckIfMembershipExists(Guid idMembership)
    {
        return !(await _membershipRepository.GetMembershipByIdAsync(idMembership) is null);
    }
}