using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlexAirFit.Database.Repositories;

public class BonusRepository : IBonusRepository
{
    private readonly FlexAirFitDbContext _context;
    private readonly ILogger _logger = Log.ForContext<BonusRepository>();

    public BonusRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddBonusAsync(Bonus bonus)
    {
        try
        {
            await _context.Bonuses.AddAsync(BonusConverter.CoreToDbModel(bonus));
            await _context.SaveChangesAsync();
            _logger.Information($"Bonus with ID {bonus.Id} was successfully added for client with ID {bonus.IdClient}.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while adding bonus with ID {bonus.Id} for client with ID {bonus.IdClient}.");
        }
    }

    public async Task<Bonus> UpdateBonusAsync(Bonus bonus)
    {
        try
        {
            var bonusDbModel = await _context.Bonuses.FindAsync(bonus.Id);
            bonusDbModel.IdClient = bonus.IdClient;
            bonusDbModel.Count = bonus.Count;

            await _context.SaveChangesAsync();
            _logger.Information($"Bonus with ID {bonus.Id} for client with ID {bonus.IdClient} was successfully updated.");
            return BonusConverter.DbToCoreModel(bonusDbModel);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while updating bonus with ID {bonus.Id} for client with ID {bonus.IdClient}.");
            throw;
        }
    }

    public async Task DeleteBonusAsync(Guid id)
    {
        try
        {
            var bonusDbModel = await _context.Bonuses.FindAsync(id);
            var idClient = bonusDbModel.IdClient;
            _context.Bonuses.Remove(bonusDbModel);
            await _context.SaveChangesAsync();
            _logger.Information($"Bonus with ID {id} for client with ID {idClient} was successfully deleted.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while deleting bonus with ID {id}.");
        }
    }

    public async Task<Bonus> GetBonusByIdAsync(Guid id)
    {
        var bonusDbModel = await _context.Bonuses.FindAsync(id);
        return BonusConverter.DbToCoreModel(bonusDbModel);
    }

    public async Task<List<Bonus>> GetBonusesAsync(int? limit, int? offset)
    {
        try
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
            var bonuses = bonusDbModels.Select(BonusConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {bonuses.Count} bonuses" + (limit.HasValue ? $" with limit {limit.Value}" : "") + (offset.HasValue ? $" and offset {offset.Value}" : ""));

            return bonuses;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving bonuses.");
            throw;
        }
    }

    public async Task<int> GetCountBonusByIdClientAsync(Guid id)
    {
        try
        {
            BonusDbModel bonusDbModel = await _context.Bonuses.FirstOrDefaultAsync(b => b.IdClient == id);
            if (bonusDbModel == null)
            {
                _logger.Error($"Bonus for client with ID {id} not found in the database.");
                return 0;
            }

            var count = BonusConverter.DbToCoreModel(bonusDbModel).Count;
            _logger.Information($"Bonus count {count} for client with ID {id} was successfully retrieved.");
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while retrieving bonus count for client with ID {id}.");
            throw;
        }
    }

    public async Task UpdateCountBonusByIdClientAsync(Guid idClient, int newCount)
    {
        try
        {
            BonusDbModel bonus = await _context.Bonuses.FirstOrDefaultAsync(b => b.IdClient == idClient);
            if (bonus == null)
            {
                _logger.Error($"Bonus for client with ID {idClient} not found in the database.");
                return;
            }

            bonus.Count = newCount;
            await _context.SaveChangesAsync();
            _logger.Information($"Bonus count for client with ID {idClient} was successfully updated to {newCount}.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while updating bonus count for client with ID {idClient}.");
        }
    }
}
