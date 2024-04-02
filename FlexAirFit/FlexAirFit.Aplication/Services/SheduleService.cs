using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.Services;

public class SheduleService(ISheduleRepository sheduleRepository, 
                            IWorkoutRepository workoutRepository) : ISheduleService
{
    private readonly ISheduleRepository _sheduleRepository = sheduleRepository;

    public async Task CreateShedule(Schedule schedule)
    {
        if (await _sheduleRepository.GetSheduleByIdAsync(schedule.Id) is not null)
        {
            throw new SheduleExistsException(schedule.Id);
        }
        await _sheduleRepository.AddSheduleAsync(schedule);
    }

    public async Task<Schedule> UpdateShedule(Schedule schedule)
    {
        if (await _sheduleRepository.GetSheduleByIdAsync(schedule.Id) is null)
        {
            throw new SheduleNotFoundException(schedule.Id);
        }
        return await _sheduleRepository.UpdateSheduleAsync(schedule);
    }

    public async Task DeleteShedule(Guid idShedule)
    {
        if (await _sheduleRepository.GetSheduleByIdAsync(idShedule) is null)
        {
            throw new SheduleNotFoundException(idShedule);
        }
        await _sheduleRepository.DeleteSheduleAsync(idShedule);
    }

    public async Task<List<Schedule>> GetSheduleByFilter(FilterShedule filter)
    {
        return await _sheduleRepository.GetSheduleByFilterAsync(filter);
    }
    
    public async Task<Schedule> GetSheduleById(Guid idShedule)
    {
        return await _sheduleRepository.GetSheduleByIdAsync(idShedule) ?? throw new SheduleNotFoundException(idShedule);
    }
    
    public async Task<List<Schedule>> GetShedules(int? limit, int? offset)
    {
        return await _sheduleRepository.GetShedulesAsync(limit, offset);
    }
}