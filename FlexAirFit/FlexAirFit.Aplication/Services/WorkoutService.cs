using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Filters;
using Serilog;

namespace FlexAirFit.Application.Services;

public class WorkoutService(IWorkoutRepository workoutRepository,
                            ITrainerRepository trainerRepository) : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository = workoutRepository;
    private readonly ILogger _logger = Log.ForContext<WorkoutService>();
    
    public async Task CreateWorkout(Workout workout)
    {
        if (await _workoutRepository.GetWorkoutByIdAsync(workout.Id) is not null)
        {
            _logger.Warning($"Workout with ID {workout.Id} already exists in the database. Skipping creation.");
            throw new WorkoutExistsException(workout.Id);
        }
        await _workoutRepository.AddWorkoutAsync(workout);
    }

    public async Task<Workout> UpdateWorkout(Workout workout)
    {
        if (await _workoutRepository.GetWorkoutByIdAsync(workout.Id) is null)
        {
            _logger.Warning($"Workout with ID {workout.Id} does not exist in the database. Skipping update.");
            throw new WorkoutNotFoundException(workout.Id);
        }
        return await _workoutRepository.UpdateWorkoutAsync(workout);
    }

    public async Task DeleteWorkout(Guid idWorkout)
    {
        if (await _workoutRepository.GetWorkoutByIdAsync(idWorkout) is null)
        {
            _logger.Warning($"Workout with ID {idWorkout} does not exist in the database. Skipping deletion.");
            throw new WorkoutNotFoundException(idWorkout);
        }
        await _workoutRepository.DeleteWorkoutAsync(idWorkout);
    }
    
    public async Task<List<Workout>> GetWorkoutByFilter(FilterWorkout filter, int? limit, int? offset)
    {
        return await _workoutRepository.GetWorkoutByFilterAsync(filter, limit, offset);
    }
    
    public async Task<Workout> GetWorkoutById(Guid idWorkout)
    {
        var workout = await _workoutRepository.GetWorkoutByIdAsync(idWorkout);
        if (workout is null)
        {
            _logger.Error($"Workout with ID {idWorkout} does not exist in the database.");
            throw new WorkoutNotFoundException(idWorkout);
        }

        _logger.Information($"Workout with ID {idWorkout} was successfully retrieved.");
        return workout;
    }

    public async Task<bool> CheckIfWorkoutExists(Guid idWorkout)
    {
        return !(await _workoutRepository.GetWorkoutByIdAsync(idWorkout) is null);
    }
    
    public async Task<List<Workout>> GetWorkouts(int? limit, int? offset)
    {
        return await _workoutRepository.GetWorkoutsAsync(limit, offset);
    }

    public async Task<string> GetWorkoutNameById(Guid id)
    {
        return await _workoutRepository.GetWorkoutNameByIdAsync(id) ?? throw new WorkoutNotFoundException(id);
    }

}