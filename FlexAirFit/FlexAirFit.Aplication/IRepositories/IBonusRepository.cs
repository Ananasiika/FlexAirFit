using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IRepositories;

public interface IBonusRepository
{
    Task AddBonusAsync(Bonus bonus);
    Task<Bonus> UpdateBonusAsync(Bonus bonus);
    Task DeleteBonusAsync(Guid id);
    Task<Bonus> GetBonusByIdAsync(Guid id);
    Task<List<Bonus>> GetBonusesAsync(int? limit, int? offset);
    Task<int> GetCountBonusByIdClientAsync(Guid id);
    Task UpdateCountBonusByIdClientAsync(Guid idClient, int newCount);
}