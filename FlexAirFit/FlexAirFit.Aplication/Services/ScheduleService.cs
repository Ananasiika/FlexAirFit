using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Filters;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace FlexAirFit.Application.Services;

public class ScheduleService(IScheduleRepository scheduleRepository, 
                            IWorkoutRepository workoutRepository,
                            IClientRepository clientRepository) : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
    private readonly IWorkoutRepository _workoutRepository = workoutRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly ILogger _logger = Log.ForContext<ScheduleService>();

    public async Task CreateSchedule(Schedule schedule)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();
        if (await _scheduleRepository.GetScheduleByIdAsync(schedule.Id) is not null)
        {
            _logger.Warning($"Schedule with ID {schedule.Id} already exists in the database. Skipping creation.");
            throw new ScheduleExistsException(schedule.Id);
        }

        var workout = await _workoutRepository.GetWorkoutByIdAsync(schedule.IdWorkout);
        if (workout is null)
        {  
            _logger.Warning($"Workout with ID {schedule.IdWorkout} does not exist in the database. Skipping creation.");
            throw new WorkoutNotFoundException(schedule.IdWorkout);
        }

        if (schedule.IdClient is not null && schedule.IdClient != Guid.Empty)
        {
            if (await _clientRepository.GetClientByIdAsync((Guid)schedule.IdClient) is null)
            {
                _logger.Warning($"Client with ID {schedule.IdClient} does not exist in the database. Skipping creation.");
                throw new ClientNotFoundException((Guid)schedule.IdClient);
            }
        }

        if (schedule.DateAndTime < DateTime.Now || schedule.DateAndTime.Hour < int.Parse(configuration["ClubOpeningTime"]) ||
            (schedule.DateAndTime.ToLocalTime() + workout.Duration).Hour > int.Parse(configuration["ClubClosingTime"]))
        {
            _logger.Warning($"Schedule time is incorrect. Skipping creation.");
            throw new ScheduleTimeIncorrectedException(schedule.Id);
        }
        if (schedule.IdClient is not null)
        {
            FilterSchedule filter_client = new FilterSchedule(null, schedule.DateAndTime - workout.Duration + TimeSpan.FromMinutes(1), schedule.DateAndTime + workout.Duration - TimeSpan.FromMinutes(1), null, schedule.IdClient, null);
            List<Schedule> schedules_client = await GetScheduleByFilter(filter_client, null, null);
            if (schedules_client is not null && schedules_client.Count != 0)
            {
                _logger.Warning($"Client with ID {schedule.IdClient} already has schedule. Skipping creation.");
                throw new ClientAlreadyHasScheduleException(schedule.Id);
            }
        }

        FilterSchedule filter_trainer = new FilterSchedule(null, schedule.DateAndTime - workout.Duration + TimeSpan.FromMinutes(1),
            schedule.DateAndTime + workout.Duration - TimeSpan.FromMinutes(1), null, null, workout.IdTrainer);
        List<Schedule> schedules_trainer = await GetScheduleByFilter(filter_trainer, null, null);
        if (schedules_trainer is not null && schedules_trainer.Count != 0)
        {
            _logger.Warning($"Trainer with ID {workout.IdTrainer} already has schedule. Skipping creation.");
            throw new TrainerAlreadyHasScheduleException(workout.IdTrainer);
        }
        
        await _scheduleRepository.AddScheduleAsync(schedule);
    }

    public async Task<Schedule> UpdateSchedule(Schedule schedule)
    {
        if (await _scheduleRepository.GetScheduleByIdAsync(schedule.Id) is null)
        {
            _logger.Warning($"Schedule with ID {schedule.Id} does not exist in the database. Skipping update.");
            throw new ScheduleNotFoundException(schedule.Id);
        }
        return await _scheduleRepository.UpdateScheduleAsync(schedule);
    }

    public async Task DeleteSchedule(Guid idSchedule)
    {
        if (await _scheduleRepository.GetScheduleByIdAsync(idSchedule) is null)
        {
            _logger.Warning($"Schedule with ID {idSchedule} does not exist in the database. Skipping deletion.");
            throw new ScheduleNotFoundException(idSchedule);
        }
        await _scheduleRepository.DeleteScheduleAsync(idSchedule);
    }

    public async Task<List<Schedule>> GetScheduleByFilter(FilterSchedule filter, int? limit, int? offset)
    {
        return await _scheduleRepository.GetScheduleByFilterAsync(filter, limit, offset);
    }
    
    public async Task<Schedule> GetScheduleById(Guid idSchedule)
    {
        var schedule = await _scheduleRepository.GetScheduleByIdAsync(idSchedule);
        if (schedule is null)
        {
            _logger.Warning($"Schedule with ID {idSchedule} does not exist in the database.");
            throw new ScheduleNotFoundException(idSchedule);
        }
        
        _logger.Information($"Schedule with ID {idSchedule} was successfully retrieved.");
        return schedule;
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