using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.Services;

public class TrainerService(ITrainerRepository trainerRepository) : ITrainerService
{
    private readonly ITrainerRepository _trainerRepository = trainerRepository;

    public async Task CreateTrainer(Trainer trainer)
    {
        if (await _trainerRepository.GetTrainerByIdAsync(trainer.Id) is not null)
        {
            throw new TrainerExistsException(trainer.Id);
        }
        await _trainerRepository.AddTrainerAsync(trainer);
    }

    public async Task<Trainer> UpdateTrainer(Trainer trainer)
    {
        if (await _trainerRepository.GetTrainerByIdAsync(trainer.Id) is null)
        {
            throw new TrainerNotFoundException(trainer.Id);
        }
        return await _trainerRepository.UpdateTrainerAsync(trainer);
    }

    public async Task DeleteTrainer(Guid idTrainer)
    {
        if (await _trainerRepository.GetTrainerByIdAsync(idTrainer) is null)
        {
            throw new TrainerNotFoundException(idTrainer);
        }
        await _trainerRepository.DeleteTrainerAsync(idTrainer);
    }
    
    public async Task<bool> CheckIfTrainerExists(Guid idTrainer)
    {
        if (await _trainerRepository.GetTrainerByIdAsync(idTrainer) is null)
        {
            return false;
        }
        return true;
    }
    
    public async Task<Trainer> GetTrainerById(Guid idTrainer)
    {
        return await _trainerRepository.GetTrainerByIdAsync(idTrainer) ?? throw new TrainerNotFoundException(idTrainer);
    }
    
    public async Task<List<Trainer>> GetTrainers(int? limit, int? offset)
    {
        return await _trainerRepository.GetTrainersAsync(limit, offset);
    }
    
    public async Task<List<Trainer>> GetTrainerByFilter(FilterTrainer filter)
    {
        return await _trainerRepository.GetTrainerByFilterAsync(filter);
    }

    public async Task<string> GetTrainerNameById(Guid id)
    {
        return await _trainerRepository.GetTrainerNameByIdAsync(id) ?? throw new TrainerNotFoundException(id);
    }
}