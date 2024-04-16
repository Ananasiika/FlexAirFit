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

public class ScheduleRepository : IScheduleRepository
{
    private readonly FlexAirFitDbContext _context;

    public ScheduleRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddScheduleAsync(Schedule schedule)
    {
        await _context.Schedules.AddAsync(ScheduleConverter.CoreToDbModel(schedule));
        await _context.SaveChangesAsync();
    }

    public async Task<Schedule> UpdateScheduleAsync(Schedule schedule)
    {
        var scheduleDbModel = await _context.Schedules.FindAsync(schedule.Id);
        scheduleDbModel.IdWorkout = schedule.IdWorkout;
        scheduleDbModel.DateAndTime = schedule.DateAndTime;
        scheduleDbModel.IdClient = schedule.IdClient;

        await _context.SaveChangesAsync();
        return ScheduleConverter.DbToCoreModel(scheduleDbModel);
    }

    public async Task DeleteScheduleAsync(Guid id)
    {
        var scheduleDbModel = await _context.Schedules.FindAsync(id);
        _context.Schedules.Remove(scheduleDbModel);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Schedule>> GetScheduleByFilterAsync(FilterSchedule filter)
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
        if (filter?.TrainerId != null)
        {
            query = query.Where(s => s.Workout.IdTrainer == filter.TrainerId);
        }
        
        var scheduleDbModels = await query.ToListAsync();
        return scheduleDbModels.Select(ScheduleConverter.DbToCoreModel).ToList();
    }

    public async Task<Schedule> GetScheduleByIdAsync(Guid id)
    {
        var scheduleDbModel = await _context.Schedules.FindAsync(id);
        return ScheduleConverter.DbToCoreModel(scheduleDbModel);
    }

    public async Task<List<Schedule>> GetSchedulesAsync(int? limit, int? offset = null )
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
        return scheduleDbModels.Select(ScheduleConverter.DbToCoreModel).ToList();
    }
}

