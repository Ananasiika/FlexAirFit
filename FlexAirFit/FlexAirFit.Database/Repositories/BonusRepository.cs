using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlexAirFit.Database.Repositories;

public class BonusRepository : IBonusRepository
{
    private readonly FlexAirFitDbContext _context;

    public BonusRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddBonusAsync(Bonus bonus)
    {
        await _context.Bonuses.AddAsync(BonusConverter.CoreToDbModel(bonus));
        await _context.SaveChangesAsync();
    }

    public async Task<Bonus> UpdateBonusAsync(Bonus bonus)
    {
        var bonusDbModel = await _context.Bonuses.FindAsync(bonus.Id);
        bonusDbModel.IdClient = bonus.IdClient;
        bonusDbModel.Count = bonus.Count;
        
        await _context.SaveChangesAsync();
        return BonusConverter.DbToCoreModel(bonusDbModel);
    }

    public async Task DeleteBonusAsync(Guid id)
    {
        var bonusDbModel = await _context.Bonuses.FindAsync(id);
        if (bonusDbModel != null)
        {
            _context.Bonuses.Remove(bonusDbModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Bonus> GetBonusByIdAsync(Guid id)
    {
        var bonusDbModel = await _context.Bonuses.FindAsync(id);
        return BonusConverter.DbToCoreModel(bonusDbModel);
    }

    public async Task<List<Bonus>> GetBonusesAsync(int? limit, int? offset)
    {
        var query = _context.Bonuses.AsQueryable();

        if (offset.HasValue)
        {
            query = query.Skip(offset.Value);
        }
        if (limit.HasValue)
        {
            query = query.Take(limit.Value);
        }

        var bonusDbModels = await query.ToListAsync();
        return bonusDbModels.Select(BonusConverter.DbToCoreModel).ToList();
    }

    public async Task<int> GetCountBonusByIdClientAsync(Guid id)
    {
        return await _context.Bonuses.CountAsync(b => b.IdClient == id);
    }

    public async Task UpdateCountBonusByIdClientAsync(Guid idClient, int newCount)
    {
        var bonus = await _context.Bonuses.FirstOrDefaultAsync(b => b.IdClient == idClient);

        if (bonus != null)
        {
            bonus.Count = newCount;
            await _context.SaveChangesAsync();
        }
    }
}
