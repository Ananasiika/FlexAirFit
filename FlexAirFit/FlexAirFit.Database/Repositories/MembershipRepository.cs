using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Repositories;

public class MembershipRepository : IMembershipRepository
{
    private readonly FlexAirFitDbContext _context;

    public MembershipRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddMembershipAsync(Membership membership)
    {
        await _context.Memberships.AddAsync(MembershipConverter.CoreToDbModel(membership));
        await _context.SaveChangesAsync();
    }

    public async Task<Membership> UpdateMembershipAsync(Guid membershipId, string? name, TimeSpan? duration, int? price, int? freezing)
    {
        var membershipDbModel = await _context.Memberships.FindAsync(membershipId);
        membershipDbModel.Name = (name is null) ? membershipDbModel.Name : name;
        membershipDbModel.Duration = (duration == TimeSpan.Zero) ? membershipDbModel.Duration : duration;
        membershipDbModel.Price = (price == 0) ? membershipDbModel.Price : price;
        membershipDbModel.Freezing = (freezing == 0) ? membershipDbModel.Freezing : freezing;

        await _context.SaveChangesAsync();
        return MembershipConverter.DbToCoreModel(membershipDbModel);
    }

    public async Task DeleteMembershipAsync(Guid id)
    {
        var membershipDbModel = await _context.Memberships.FindAsync(id);
        _context.Memberships.Remove(membershipDbModel);
        await _context.SaveChangesAsync();
    }

    public async Task<Membership> GetMembershipByIdAsync(Guid id)
    {
        var membershipDbModel = await _context.Memberships.FindAsync(id);
        return MembershipConverter.DbToCoreModel(membershipDbModel);
    }

    public async Task<List<Membership>> GetMembershipsAsync(int? limit, int? offset = null)
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
        return membershipsDbModels.Select(MembershipConverter.DbToCoreModel).ToList();
    }
}
