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

public class TrainerRepository : ITrainerRepository
{
    private readonly FlexAirFitDbContext _context;

    public TrainerRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddTrainerAsync(Trainer trainer)
    {
        await _context.Trainers.AddAsync(TrainerConverter.CoreToDbModel(trainer));
        await _context.SaveChangesAsync();
        
    }

    public async Task<Trainer> UpdateTrainerAsync(Trainer trainer)
    {
        var trainerDbModel = await _context.Trainers.FindAsync(trainer.Id);
        trainerDbModel.Name = trainer.Name;
        trainerDbModel.Gender = trainer.Gender;
        trainerDbModel.Specialization = trainer.Specialization;
        trainerDbModel.Experience = trainer.Experience;
        trainerDbModel.Rating = trainer.Rating;

        await _context.SaveChangesAsync();
        return TrainerConverter.DbToCoreModel(trainerDbModel);
    }

    public async Task DeleteTrainerAsync(Guid id)
    {
        var trainerDbModel = await _context.Trainers.FindAsync(id);
        _context.Trainers.Remove(trainerDbModel);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Trainer>> GetTrainerByFilterAsync(FilterTrainer filter)
    {
        var query = _context.Trainers.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(t => t.Name.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        if (!string.IsNullOrEmpty(filter.Gender))
        {
            query = query.Where(t => t.Gender.Equals(filter.Gender, StringComparison.InvariantCultureIgnoreCase));
        }

        if (!string.IsNullOrEmpty(filter.Specialization))
        {
            query = query.Where(t => t.Specialization.Contains(filter.Specialization, StringComparison.InvariantCultureIgnoreCase));
        }

        if (filter.MinExperience.HasValue)
        {
            query = query.Where(t => t.Experience >= filter.MinExperience.Value);
        }

        if (filter.MaxExperience.HasValue)
        {
            query = query.Where(t => t.Experience <= filter.MaxExperience.Value);
        }

        if (filter.MinRating.HasValue)
        {
            query = query.Where(t => t.Rating >= filter.MinRating.Value);
        }

        if (filter.MaxRating.HasValue)
        {
            query = query.Where(t => t.Rating <= filter.MaxRating.Value);
        }

        var trainerDbModels = await query.ToListAsync();
        return trainerDbModels.Select(TrainerConverter.DbToCoreModel).ToList();
    }

    public async Task<Trainer> GetTrainerByIdAsync(Guid id)
    {
        var trainerDbModel = await _context.Trainers.FindAsync(id);
        return TrainerConverter.DbToCoreModel(trainerDbModel);
    }

    public async Task<List<Trainer>> GetTrainersAsync(int? limit, int? offset = null)
    {
        var query = _context.Trainers.AsQueryable();

        if (offset.HasValue)
        {
            query = query.Skip(offset.Value);
        }

        if (limit.HasValue)
        {
            query = query.Take(limit.Value);
        }

        var trainersDbModels = await query.ToListAsync();
        return trainersDbModels.Select(TrainerConverter.DbToCoreModel).ToList();
    }

    public async Task<string> GetTrainerNameByIdAsync(Guid id)
    {
        var trainerDbModel = await _context.Trainers.FindAsync(id);
        return trainerDbModel?.Name;
    }
}
