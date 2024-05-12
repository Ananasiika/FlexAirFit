using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.IRepositories;

public interface IWorkoutRepository
{
    Task AddWorkoutAsync(Workout workout);
    Task<Workout> UpdateWorkoutAsync(Workout workout);
    Task DeleteWorkoutAsync(Guid id);
    Task<List<Workout>> GetWorkoutByFilterAsync(FilterWorkout filter, int? limit, int? offset);
    Task<Workout> GetWorkoutByIdAsync(Guid id);
    Task<List<Workout>> GetWorkoutsAsync(int? limit, int? offset);
    Task<string> GetWorkoutNameByIdAsync(Guid id);
}