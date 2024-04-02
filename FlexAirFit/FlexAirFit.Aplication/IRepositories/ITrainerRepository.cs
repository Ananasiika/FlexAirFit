using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.IRepositories;

public interface ITrainerRepository
{
    Task AddTrainerAsync(Trainer trainer);
    Task<Trainer> UpdateTrainerAsync(Trainer trainer);
    Task DeleteTrainerAsync(Guid id);
    Task<Trainer> GetTrainerByIdAsync(Guid id);
    Task<List<Trainer>> GetTrainersAsync(int? limit, int? offset);
    Task<List<Trainer>> GetTrainerByFilterAsync(FilterTrainer filter);
    Task<string> GetTrainerNameByIdAsync(Guid id);
}