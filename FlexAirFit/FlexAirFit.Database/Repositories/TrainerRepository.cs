using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlexAirFit.Database.Repositories;

public class TrainerRepository : ITrainerRepository
{
    private readonly FlexAirFitDbContext _context;
    private readonly ILogger _logger = Log.ForContext<TrainerRepository>();

    public TrainerRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddTrainerAsync(Trainer trainer)
    {
        try
        {
            await _context.Trainers.AddAsync(TrainerConverter.CoreToDbModel(trainer));
            await _context.SaveChangesAsync();
            _logger.Information($"Trainer with ID {trainer.Id} was successfully added.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while adding trainer with ID {trainer.Id}.");
        }
    }

    public async Task<Trainer> UpdateTrainerAsync(Trainer trainer)
    {
        try
        {
            var trainerDbModel = await _context.Trainers.FindAsync(trainer.Id);
            
            trainerDbModel.Name = trainer.Name;
            trainerDbModel.Gender = trainer.Gender;
            trainerDbModel.Specialization = trainer.Specialization;
            trainerDbModel.Experience = trainer.Experience;
            trainerDbModel.Rating = trainer.Rating;

            await _context.SaveChangesAsync();
            _logger.Information($"Trainer with ID {trainer.Id} was successfully updated.");
            return TrainerConverter.DbToCoreModel(trainerDbModel);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while updating trainer with ID {trainer.Id}.");
            throw;
        }
    }

    public async Task DeleteTrainerAsync(Guid id)
    {
        try
        {
            var trainerDbModel = await _context.Trainers.FindAsync(id);
            _context.Trainers.Remove(trainerDbModel);
            await _context.SaveChangesAsync();
            _logger.Information($"Trainer with ID {id} was successfully deleted.");
            
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while deleting trainer with ID {id}.");
        }
    }

    public async Task<List<Trainer>> GetTrainerByFilterAsync(FilterTrainer filter, int? limit, int? offset)
    {
        try
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

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var trainerDbModels = await query.ToListAsync();
            var trainers = trainerDbModels.Select(TrainerConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {trainers.Count} trainers with filter: " +
                                $"Name - {filter.Name}, Gender - {filter.Gender}, Specialization - {filter.Specialization}, " +
                                $"Min Experience - {filter.MinExperience}, Max Experience - {filter.MaxExperience}, " +
                                $"Min Rating - {filter.MinRating}, Max Rating - {filter.MaxRating}, Limit - {limit.Value}, Offset - {offset.Value}");

            return trainers;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving trainers by filter.");
            throw;
        }
    }

    public async Task<Trainer> GetTrainerByIdAsync(Guid id)
    {
        var trainerDbModel = await _context.Trainers.FindAsync(id);
        return TrainerConverter.DbToCoreModel(trainerDbModel);
    }

    public async Task<List<Trainer>> GetTrainersAsync(int? limit, int? offset = null)
    {
        try
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
            var trainers = trainersDbModels.Select(TrainerConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {trainers.Count} trainers" + (limit.HasValue ? $" with limit {limit.Value}" : "") + (offset.HasValue ? $" and offset {offset.Value}" : ""));

            return trainers;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving trainers.");
            throw;
        }
    }

    public async Task<string> GetTrainerNameByIdAsync(Guid id)
    {
        try
        {
            var trainerDbModel = await _context.Trainers.FindAsync(id);
            if (trainerDbModel == null)
            {
                _logger.Error($"Trainer with ID {id} not found in the database.");
                return null;
            }

            _logger.Information($"Trainer name with ID {id} was successfully retrieved.");
            return trainerDbModel.Name;
        }
        catch (Exception ex)

        {
            _logger.Error(ex, $"An error occurred while retrieving trainer name with ID {id}.");
            throw;
        }
    }
}
