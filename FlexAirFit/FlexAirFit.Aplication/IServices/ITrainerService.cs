using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;

namespace FlexAirFit.Application.IServices;

public interface ITrainerService
{
    Task CreateTrainer(Trainer trainer);
    Task<Trainer> UpdateTrainer(Trainer trainer);
    Task DeleteTrainer(Guid id);
    Task<Trainer> GetTrainerById(Guid id);
    Task<List<Trainer>> GetTrainers(int? limit, int? offset);
    Task<List<Trainer>> GetTrainerByFilter(FilterTrainer filter, int? limit, int? offset);
    Task<string> GetTrainerNameById(Guid id);
    Task<bool> CheckIfTrainerExists(Guid trainerId);
}