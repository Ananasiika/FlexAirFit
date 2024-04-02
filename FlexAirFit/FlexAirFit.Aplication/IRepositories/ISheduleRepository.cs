using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.IRepositories;

public interface ISheduleRepository
{
    Task AddSheduleAsync(Schedule schedule);
    Task<Schedule> UpdateSheduleAsync(Schedule schedule);
    Task DeleteSheduleAsync(Guid id);
    Task<List<Schedule>> GetSheduleByFilterAsync(FilterShedule filter);
    Task<Schedule> GetSheduleByIdAsync(Guid id);
    Task<List<Schedule>> GetShedulesAsync(int? limit, int? offset);
}