using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IRepositories;

public interface IClientRepository
{
    Task AddClientAsync(Client client);
    Task<Client> UpdateClientAsync(Client client);
    Task DeleteClientAsync(Guid id);
    Task<Client> GetClientByIdAsync(Guid id);
    Task<List<Client>> GetClientsAsync(int? limit, int? offset);
    Task<DateOnly> GetMembershipEndDateAsync(Guid idClient);
}