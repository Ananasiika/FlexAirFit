using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.IServices;

public interface ISheduleService
{
    Task CreateShedule(Schedule schedule);
    Task<Schedule> UpdateShedule(Schedule schedule);
    Task DeleteShedule(Guid id);
    Task<List<Schedule>> GetSheduleByFilter(FilterShedule filter);
    Task<Schedule> GetSheduleById(Guid id);
    Task<List<Schedule>> GetShedules(int? limit, int? offset);
}