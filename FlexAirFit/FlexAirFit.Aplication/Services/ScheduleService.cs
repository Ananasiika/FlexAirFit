using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Filters;
using Microsoft.Extensions.Configuration;

namespace FlexAirFit.Application.Services;

public class ScheduleService(IScheduleRepository scheduleRepository, 
                            IWorkoutRepository workoutRepository,
                            IClientRepository clientRepository) : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
    private readonly IWorkoutRepository _workoutRepository = workoutRepository;
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task CreateSchedule(Schedule schedule)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();
        if (await _scheduleRepository.GetScheduleByIdAsync(schedule.Id) is not null)
        {
            throw new ScheduleExistsException(schedule.Id);
        }

        var workout = await _workoutRepository.GetWorkoutByIdAsync(schedule.IdWorkout);
        if (workout is null)
        {
            throw new WorkoutNotFoundException(schedule.IdWorkout);
        }

        if (schedule.IdClient is not null)
        {
            if (await _clientRepository.GetClientByIdAsync((Guid)schedule.IdClient) is null)
            {
                throw new ClientNotFoundException((Guid)schedule.IdClient);
            }
        }

        if (schedule.DateAndTime < DateTime.Now || schedule.DateAndTime.Hour < int.Parse(configuration["ClubOpeningTime"]) ||
            (schedule.DateAndTime - workout.Duration).Hour > int.Parse(configuration["ClubClosingTime"]))
        {
            throw new ScheduleTimeIncorrectedException(schedule.Id);
        }

        if (schedule.IdClient is not null)
        {
            FilterSchedule filter_client = new FilterSchedule(null, schedule.DateAndTime, schedule.DateAndTime + workout.Duration - TimeSpan.FromMinutes(1), schedule.IdClient, null);
            List<Schedule> schedules_client = await GetScheduleByFilter(filter_client);
            if (schedules_client is not null && schedules_client.Count != 0)
            {
                throw new ClientAlreadyHasScheduleException(schedule.Id);
            }
        }

        FilterSchedule filter_trainer = new FilterSchedule(null, schedule.DateAndTime,
            schedule.DateAndTime + workout.Duration - TimeSpan.FromMinutes(1), null, workout.IdTrainer);
        List<Schedule> schedules_trainer = await GetScheduleByFilter(filter_trainer);
        if (schedules_trainer is not null && schedules_trainer.Count != 0)
        {
            throw new TrainerAlreadyHasScheduleException(workout.IdTrainer);
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