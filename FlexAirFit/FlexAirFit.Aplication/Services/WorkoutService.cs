using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.Services;

public class WorkoutService(IWorkoutRepository workoutRepository,
                            ITrainerRepository trainerRepository) : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository = workoutRepository;
    
    public async Task CreateWorkout(Workout workout)
    {
        if (await _workoutRepository.GetWorkoutByIdAsync(workout.Id) is not null)
        {
            throw new WorkoutExistsException(workout.Id);
        }
        await _workoutRepository.AddWorkoutAsync(workout);
    }

    public async Task<Workout> UpdateWorkout(Workout workout)
    {
        if (await _workoutRepository.GetWorkoutByIdAsync(workout.Id) is null)
        {
            throw new WorkoutNotFoundException(workout.Id);
        }
        return await _workoutRepository.UpdateWorkoutAsync(workout);
    }

    public async Task DeleteWorkout(Guid idWorkout)
    {
        if (await _workoutRepository.GetWorkoutByIdAsync(idWorkout) is null)
        {
            throw new WorkoutNotFoundException(idWorkout);
        }
        await _workoutRepository.DeleteWorkoutAsync(idWorkout);
    }
    
    public async Task<List<Workout>> GetWorkoutByFilter(FilterWorkout filter)
    {
        return await _workoutRepository.GetWorkoutByFilterAsync(filter);
    }
    
    public async Task<Workout> GetWorkoutById(Guid idWorkout)
    {
        return await _workoutRepository.GetWorkoutByIdAsync(idWorkout) ?? throw new WorkoutNotFoundException(idWorkout);
    }

    public async Task<bool> CheckIfWorkoutExists(Guid idWorkout)
    {
        if (await _workoutRepository.GetWorkoutByIdAsync(idWorkout) is null)
        {
            return false;
        }
        return true;
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