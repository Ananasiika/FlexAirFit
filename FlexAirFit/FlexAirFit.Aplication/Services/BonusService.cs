using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Serilog;

namespace FlexAirFit.Application.Services;

public class BonusService(IBonusRepository bonusRepository) : IBonusService
{
    private readonly IBonusRepository _bonusRepository = bonusRepository;
    private readonly ILogger _logger = Log.ForContext<BonusService>();

    public async Task CreateBonus(Bonus bonus)
    {
        if (await _bonusRepository.GetBonusByIdAsync(bonus.Id) is not null)
        {
            _logger.Warning($"Bonus with ID {bonus.Id} already exists in the database. Skipping creation.");
            throw new BonusExistsException(bonus.Id);
        }

        await _bonusRepository.AddBonusAsync(bonus);
    }

    public async Task<Bonus> UpdateBonus(Bonus bonus)
    {
        if (await _bonusRepository.GetBonusByIdAsync(bonus.Id) is null)
        {
            _logger.Warning($"Bonus with ID {bonus.Id} not found in the database. Skipping update.");
            throw new BonusNotFoundException(bonus.Id);
        }
        return await _bonusRepository.UpdateBonusAsync(bonus);
    }

    public async Task DeleteBonus(Guid idBonus)
    {
        if (await _bonusRepository.GetBonusByIdAsync(idBonus) is null)
        {
            _logger.Warning($"Bonus with ID {idBonus} not found in the database. Skipping deletion.");
            throw new BonusNotFoundException(idBonus);
        }
        await _bonusRepository.DeleteBonusAsync(idBonus);
    }
    
    public async Task<Bonus> GetBonusById(Guid idBonus)
    {
        _logger.Information($"Bonus with ID {idBonus} was successfully retrieved.");
        return await _bonusRepository.GetBonusByIdAsync(idBonus) ?? throw new BonusNotFoundException(idBonus);
    }
    
    public async Task<List<Bonus>> GetBonuses(int? limit, int? offset)
    {
        return await _bonusRepository.GetBonusesAsync(limit, offset);
    }

    public async Task<int> GetCountBonusByIdClient(Guid id)
    {
        return await _bonusRepository.GetCountBonusByIdClientAsync(id);
    }
    
    public async Task UpdateCountBonusByIdClient(Guid idClient, int newCount)
    {
        await _bonusRepository.UpdateCountBonusByIdClientAsync(idClient, newCount);
    }
}