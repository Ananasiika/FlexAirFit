using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.IRepositories;

public interface IScheduleRepository
{
    Task AddScheduleAsync(Schedule schedule);
    Task<Schedule> UpdateScheduleAsync(Schedule schedule);
    Task DeleteScheduleAsync(Guid id);
    Task<List<Schedule>> GetScheduleByFilterAsync(FilterSchedule filter);
    Task<Schedule> GetScheduleByIdAsync(Guid id);
    Task<List<Schedule>> GetSchedulesAsync(int? limit, int? offset);
}