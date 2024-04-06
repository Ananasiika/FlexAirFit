using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly FlexAirFitDbContext _context;

    public WorkoutRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }
    
    public async Task AddWorkoutAsync(Workout workout)
    {
        await _context.Workouts.AddAsync(WorkoutConverter.CoreToDbModel(workout)!);
        await _context.SaveChangesAsync();
    }

    public async Task<Workout> UpdateWorkoutAsync(Workout workout)
    {
        var workoutDbModel = await _context.Workouts.FindAsync(workout.Id);
        workoutDbModel!.Name = workout.Name;
        workoutDbModel!.Description = workout.Description;
        workoutDbModel!.IdTrainer = workout.IdTrainer;
        workoutDbModel!.Duration = workout.Duration;
        workoutDbModel!.Level = workout.Level;

        await _context.SaveChangesAsync();
        return WorkoutConverter.DbToCoreModel(workoutDbModel)!;
    }

    public async Task DeleteWorkoutAsync(Guid id)
    {
        var workoutDbModel = await _context.Workouts.FindAsync(id);
        _context.Workouts.Remove(workoutDbModel!);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Workout>> GetWorkoutByFilterAsync(FilterWorkout filter)
    {
        var query = _context.Workouts.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(w => w.Name.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        if (!string.IsNullOrEmpty(filter.NameTrainer))
        {
            query = query.Include(w => w.Trainer)
                .Where(w => w.Trainer.Name.Contains(filter.NameTrainer, StringComparison.InvariantCultureIgnoreCase));
        }

        if (filter.MinDuration.HasValue)
        {
            query = query.Where(w => w.Duration >= filter.MinDuration.Value);
        }

        if (filter.MaxDuration.HasValue)
        {
            query = query.Where(w => w.Duration <= filter.MaxDuration.Value);
        }

        if (filter.MinLevel.HasValue)
        {
            query = query.Where(w => w.Level >= filter.MinLevel.Value);
        }

        if (filter.MaxLevel.HasValue)
        {
            query = query.Where(w => w.Level <= filter.MaxLevel.Value);
        }

        var workoutDbModels = await query.ToListAsync();
        return workoutDbModels.Select(WorkoutConverter.DbToCoreModel).ToList();
    }


    public async Task<Workout> GetWorkoutByIdAsync(Guid id)
    {
        var workoutDbModel = await _context.Workouts.FindAsync(id);
        return WorkoutConverter.DbToCoreModel(workoutDbModel);
    }

    public async Task<List<Workout>> GetWorkoutsAsync(int? limit, int? offset = null)
    {
        var query = _context.Workouts.AsQueryable();

        if (offset.HasValue)
        {
            query = query.Skip(offset.Value);
        }

        if (limit.HasValue)
        {
            query = query.Take(limit.Value);
        }

        var workoutsDbModels = await query.ToListAsync();
        return workoutsDbModels.Select(WorkoutConverter.DbToCoreModel).ToList();
    }

    public async Task<string> GetWorkoutNameByIdAsync(Guid id)
    {
        var workoutDbModel = await _context.Workouts.FindAsync(id);
        return workoutDbModel?.Name;
    }
}

