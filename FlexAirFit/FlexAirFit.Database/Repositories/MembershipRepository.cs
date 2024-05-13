using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlexAirFit.Database.Repositories;

public class MembershipRepository : IMembershipRepository
{
    private readonly FlexAirFitDbContext _context;
    private readonly ILogger _logger = Log.ForContext<MembershipRepository>();

    public MembershipRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddMembershipAsync(Membership membership)
    {
        try
        {
            await _context.Memberships.AddAsync(MembershipConverter.CoreToDbModel(membership));
            await _context.SaveChangesAsync();
            _logger.Information($"Membership with ID {membership.Id} was successfully added.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while adding membership with ID {membership.Id}.");
        }
    }

    public async Task<Membership> UpdateMembershipAsync(Membership membership)
    {
        try
        {
            var membershipDbModel = await _context.Memberships.FindAsync(membership.Id);
            membershipDbModel.Name = membership.Name;
            membershipDbModel.Duration = membership.Duration;
            membershipDbModel.Price = membership.Price;
            membershipDbModel.Freezing = membership.Freezing;

            await _context.SaveChangesAsync();
            _logger.Information($"Membership with ID {membership.Id} was successfully updated.");
            return MembershipConverter.DbToCoreModel(membershipDbModel);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while updating membership with ID {membership.Id}.");
            throw;
        }
    }

    public async Task DeleteMembershipAsync(Guid id)
    {
        try
        {
            var membershipDbModel = await _context.Memberships.FindAsync(id);
            _context.Memberships.Remove(membershipDbModel);
            await _context.SaveChangesAsync(); 
            _logger.Information($"Membership with ID {id} was successfully deleted.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while deleting membership with ID {id}.");
        }
    }

    public async Task<Membership> GetMembershipByIdAsync(Guid id)
    {
        var membershipDbModel = await _context.Memberships.FindAsync(id);
        return MembershipConverter.DbToCoreModel(membershipDbModel);
    }

    public async Task<List<Membership>> GetMembershipsAsync(int? limit, int? offset = null)
    {
        try
        {
            var query = _context.Memberships.AsQueryable();

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var membershipsDbModels = await query.ToListAsync();
            var memberships = membershipsDbModels.Select(MembershipConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {memberships.Count} memberships" + (limit.HasValue ? $" with limit {limit.Value}" : "") + (offset.HasValue ? $" and offset {offset.Value}" : ""));

            return memberships;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving memberships.");
            throw;
        }
    }
}
