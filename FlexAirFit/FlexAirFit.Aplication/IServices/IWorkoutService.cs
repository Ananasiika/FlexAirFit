using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.IRepositories;

public interface IWorkoutService
{
    Task CreateWorkout(Workout workout);
    Task<Workout> UpdateWorkout(Guid idWorkout, string? name, string? description, Guid? idTrainer, TimeSpan? duration, int? level);
    Task DeleteWorkout(Guid id);
    Task<List<Workout>> GetWorkoutByFilter(FilterWorkout filter);
    Task<Workout> GetWorkoutById(Guid id);
    Task<List<Workout>> GetWorkouts(int? limit, int? offset);
    Task<string> GetWorkoutNameById(Guid id);
    Task<bool> CheckIfWorkoutExists(Guid workoutId);
}
