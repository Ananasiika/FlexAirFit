using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.IServices;

public interface IScheduleService
{
    Task CreateSchedule(Schedule schedule);
    Task<Schedule> UpdateSchedule(Schedule schedule);
    Task DeleteSchedule(Guid id);
    Task<List<Schedule>> GetScheduleByFilter(FilterSchedule filter);
    Task<Schedule> GetScheduleById(Guid id);
    Task<List<Schedule>> GetSchedules(int? limit, int? offset);
    Task<bool> CheckIfScheduleExists(Guid idSchedule);
}