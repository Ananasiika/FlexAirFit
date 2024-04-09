﻿using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.IRepositories;

public interface IWorkoutRepository
{
    Task AddWorkoutAsync(Workout workout);
    Task<Workout> UpdateWorkoutAsync(string? name, string? description, Guid? idTrainer, TimeSpan? duration, int? level);
    Task DeleteWorkoutAsync(Guid id);
    Task<List<Workout>> GetWorkoutByFilterAsync(FilterWorkout filter);
    Task<Workout> GetWorkoutByIdAsync(Guid id);
    Task<List<Workout>> GetWorkoutsAsync(int? limit, int? offset);
    Task<string> GetWorkoutNameByIdAsync(Guid id);
}
