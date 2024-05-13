using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlexAirFit.Database.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly FlexAirFitDbContext _context;
    private readonly ILogger _logger = Log.ForContext<WorkoutRepository>();

    public WorkoutRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddWorkoutAsync(Workout workout)
    {
        try
        {
            await _context.Workouts.AddAsync(WorkoutConverter.CoreToDbModel(workout)!);
            await _context.SaveChangesAsync();
            _logger.Information($"Workout with ID {workout.Id} was successfully added.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while adding workout with ID {workout.Id}.");
        }
    }

    public async Task<Workout> UpdateWorkoutAsync(Workout workout)
    {
        try
        {
            var workoutDbModel = await _context.Workouts.FindAsync(workout.Id);
            workoutDbModel.Name = workout.Name;
            workoutDbModel.Description = workout.Description;
            workoutDbModel.IdTrainer = workout.IdTrainer;
            workoutDbModel.Duration = workout.Duration;
            workoutDbModel.Level = workout.Level;

            await _context.SaveChangesAsync();
            _logger.Information($"Workout with ID {workout.Id} was successfully updated.");
            return WorkoutConverter.DbToCoreModel(workoutDbModel)!;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while updating workout with ID {workout.Id}.");
            throw;
        }
    }

    public async Task DeleteWorkoutAsync(Guid id)
    {
        try
        {
            var workoutDbModel = await _context.Workouts.FindAsync(id);
            _context.Workouts.Remove(workoutDbModel);
            await _context.SaveChangesAsync();
            _logger.Information($"Workout with ID {id} was successfully deleted.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while deleting workout with ID {id}.");
        }
    }

    public async Task<List<Workout>> GetWorkoutByFilterAsync(FilterWorkout filter, int? limit, int? offset)
    {
        try
        {
            var query = _context.Workouts.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(w => EF.Functions.Like(w.Name, $"%{filter.Name}%"));
            }

            if (!string.IsNullOrEmpty(filter.NameTrainer))
            {
                query = query.Include(w => w.Trainer)
                    .Where(w => EF.Functions.Like(w.Trainer.Name, $"%{filter.NameTrainer}%"));
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

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var workoutDbModels = await query.ToListAsync();
            var workouts = workoutDbModels.Select(WorkoutConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {workouts.Count} workouts with filter: " +
                                $"Name - {filter.Name}, Trainer Name - {filter.NameTrainer}, " +
                                $"Min Duration - {filter.MinDuration}, Max Duration - {filter.MaxDuration}, " +
                                $"Min Level - {filter.MinLevel}, Max Level - {filter.MaxLevel}, Limit - {limit.Value}, Offset - {offset.Value}");

            return workouts;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving workouts by filter.");
            throw;
        }
    }

    public async Task<Workout> GetWorkoutByIdAsync(Guid id)
    {
        var workoutDbModel = await _context.Workouts.FindAsync(id);
        return WorkoutConverter.DbToCoreModel(workoutDbModel);
    }

    public async Task<List<Workout>> GetWorkoutsAsync(int? limit, int? offset = null)
    {
        try
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
            var workouts = workoutsDbModels.Select(WorkoutConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {workouts.Count} workouts" + (limit.HasValue ? $" with limit {limit.Value}" : "") + (offset.HasValue ? $" and offset {offset.Value}" : ""));

            return workouts;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving workouts.");
            throw;
        }
    }

    public async Task<string> GetWorkoutNameByIdAsync(Guid id)
    {
        try
        {
            var workoutDbModel = await _context.Workouts.FindAsync(id);
            if (workoutDbModel == null)
            {
                return null;
            }

            return workoutDbModel.Name;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while retrieving workout name with ID {id}.");
            throw;
        }
    }
}
