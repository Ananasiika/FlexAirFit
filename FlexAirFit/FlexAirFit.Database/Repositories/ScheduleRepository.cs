using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlexAirFit.Database.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly FlexAirFitDbContext _context;
    private readonly ILogger _logger = Log.ForContext<ScheduleRepository>();

    public ScheduleRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddScheduleAsync(Schedule schedule)
    {
        try
        {
            await _context.Schedules.AddAsync(ScheduleConverter.CoreToDbModel(schedule));
            await _context.SaveChangesAsync();
            _logger.Information($"Schedule with ID {schedule.Id} was successfully added.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message, $"An error occurred while adding schedule with ID {schedule.Id}.");
            _logger.Error(ex.InnerException.Message, $"An error occurred while adding schedule with ID {schedule.Id}.");
        }
    }

    public async Task<Schedule> UpdateScheduleAsync(Schedule schedule)
    {
        try
        {
            var scheduleDbModel = await _context.Schedules.FindAsync(schedule.Id);

            scheduleDbModel.IdWorkout = schedule.IdWorkout;
            scheduleDbModel.DateAndTime = schedule.DateAndTime;
            scheduleDbModel.IdClient = schedule.IdClient;

            await _context.SaveChangesAsync();
            _logger.Information($"Schedule with ID {schedule.Id} was successfully updated.");
            return ScheduleConverter.DbToCoreModel(scheduleDbModel);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while updating schedule with ID {schedule.Id}.");
            throw;
        }
    }

    public async Task DeleteScheduleAsync(Guid id)
    {
        try
        {
            var scheduleDbModel = await _context.Schedules.FindAsync(id);
            _context.Schedules.Remove(scheduleDbModel);
            await _context.SaveChangesAsync();
            _logger.Information($"Schedule with ID {id} was successfully deleted.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while deleting schedule with ID {id}.");
        }
    }

    public async Task<List<Schedule>> GetScheduleByFilterAsync(FilterSchedule filter, int? limit, int? offset)
    {
        try
        {
            var query = _context.Schedules
                .Include(s => s.Workout)
                .Include(s => s.Client)
                .AsQueryable();

            if (filter?.NameWorkout != null)
            {
                query = query.Where(s => s.Workout.Name.Contains(filter.NameWorkout));
            }

            if (filter?.MinDateAndTime != null)
            {
                query = query.Where(s => s.DateAndTime >= filter.MinDateAndTime);
            }

            if (filter?.MaxDateAndTime != null)
            {
                query = query.Where(s => s.DateAndTime <= filter.MaxDateAndTime);
            }

            if (filter?.ClientId != null)
            {
                query = query.Where(s => s.IdClient == filter.ClientId);
            }
            if (filter?.WorkoutType == WorkoutType.GroupWorkout)
            {
                query = query.Where(s => s.IdClient == Guid.Empty || s.IdClient == null);
            }
            if (filter?.TrainerId != null)
            {
                query = query.Where(s => s.Workout.IdTrainer == filter.TrainerId);
            }

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var scheduleDbModels = await query.ToListAsync();
            var schedules = scheduleDbModels.Select(ScheduleConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {schedules.Count} schedules with filter: " +
                                $"Name Workout - {filter?.NameWorkout}, Min Date and Time - {filter?.MinDateAndTime}, " +
                                $"Max Date and Time - {filter?.MaxDateAndTime}, Client ID - {filter?.ClientId}, " +
                                $"Workout Type - {filter?.WorkoutType}, Trainer ID - {filter?.TrainerId}" + (limit.HasValue ? $", " +
                                    $"Limit - {limit.Value}" : "") + (offset.HasValue ? $", Offset - {offset.Value}" : ""));
            return schedules;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving schedules by filter.");
            throw;
        }
    }

    public async Task<Schedule> GetScheduleByIdAsync(Guid id)
    {
        var scheduleDbModel = await _context.Schedules.FindAsync(id);
        return ScheduleConverter.DbToCoreModel(scheduleDbModel);
    }

    public async Task<List<Schedule>> GetSchedulesAsync(int? limit, int? offset = null)
    {
        try
        {
            var query = _context.Schedules.AsQueryable();

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var scheduleDbModels = await query.ToListAsync();
            var schedules = scheduleDbModels.Select(ScheduleConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {schedules.Count} schedules" + (limit.HasValue ? $" with limit {limit.Value}" : "") + (offset.HasValue ? $" and offset {offset.Value}" : ""));

            return schedules;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving schedules.");
            throw;
        }
    }
}
