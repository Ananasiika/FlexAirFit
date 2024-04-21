using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.Services;

public class ScheduleService(IScheduleRepository scheduleRepository, 
                            IWorkoutRepository workoutRepository) : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository = scheduleRepository;

    public async Task CreateSchedule(Schedule schedule)
    {
        if (await _scheduleRepository.GetScheduleByIdAsync(schedule.Id) is not null)
        {
            throw new ScheduleExistsException(schedule.Id);
        }
        await _scheduleRepository.AddScheduleAsync(schedule);
    }

    public async Task<Schedule> UpdateSchedule(Schedule schedule)
    {
        if (await _scheduleRepository.GetScheduleByIdAsync(schedule.Id) is null)
        {
            throw new ScheduleNotFoundException(schedule.Id);
        }
        return await _scheduleRepository.UpdateScheduleAsync(schedule);
    }

    public async Task DeleteSchedule(Guid idSchedule)
    {
        if (await _scheduleRepository.GetScheduleByIdAsync(idSchedule) is null)
        {
            throw new ScheduleNotFoundException(idSchedule);
        }
        await _scheduleRepository.DeleteScheduleAsync(idSchedule);
    }

    public async Task<List<Schedule>> GetScheduleByFilter(FilterSchedule filter)
    {
        return await _scheduleRepository.GetScheduleByFilterAsync(filter);
    }
    
    public async Task<Schedule> GetScheduleById(Guid idSchedule)
    {
        return await _scheduleRepository.GetScheduleByIdAsync(idSchedule) ?? throw new ScheduleNotFoundException(idSchedule);
    }
    
    public async Task<List<Schedule>> GetSchedules(int? limit, int? offset)
    {
        return await _scheduleRepository.GetSchedulesAsync(limit, offset);
    }
    
    public async Task<bool> CheckIfScheduleExists(Guid idSchedule)
    {
        return !(await _scheduleRepository.GetScheduleByIdAsync(idSchedule) is null);
    }
}