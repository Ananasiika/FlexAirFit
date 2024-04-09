using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IServices;

public interface IBonusService
{
    Task CreateBonus(Bonus bonus);
    Task<Bonus> UpdateBonus(Bonus bonus);
    Task DeleteBonus(Guid id);
    Task<Bonus> GetBonusById(Guid id);
    Task<List<Bonus>> GetBonuses(int? limit, int? offset);
    Task<int> GetCountBonusByIdClient(Guid id);
    Task UpdateCountBonusByIdClient(Guid idClient, int newCount);
}