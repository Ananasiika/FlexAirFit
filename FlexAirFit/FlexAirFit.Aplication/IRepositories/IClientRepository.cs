using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IRepositories;

public interface IClientRepository
{
    Task AddClientAsync(Client client);
    Task<Client> UpdateClientAsync(Guid idClient, string? name, string? gender, DateOnly? dateOfBirth, Guid? idMembership, DateOnly? membershipEnd, int? remainFreezing, List<Tuple<DateOnly, DateOnly>>? freezingIntervals);
    Task DeleteClientAsync(Guid id);
    Task<Client> GetClientByIdAsync(Guid id);
    Task<List<Client>> GetClientsAsync(int? limit, int? offset);
    Task<DateOnly> GetMembershipEndDateAsync(Guid idClient);
}
