using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IServices;

public interface IClientService
{
    Task CreateClient(Client client);
    Task<Client> UpdateClient(Guid idClient, string? name, string? gender, DateOnly? dateOfBirth, Guid? idMembership, DateOnly? membershipEnd, int? remainFreezing, List<Tuple<DateOnly, DateOnly>>? freezingIntervals);
    Task DeleteClient(Guid id);
    Task<Client> GetClientById(Guid id);
    Task<List<Client>> GetClients(int? limit, int? offset);
    Task FreezeMembership(Guid idClient, DateOnly FreezingStart, int durationInDays);
    Task<DateOnly> GetMembershipEndDate(Guid idClient);
    Task<bool> CheckIfClientExists(Guid idClient);
}
