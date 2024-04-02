using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;

namespace FlexAirFit.Application.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private const int MinFreezing = 7;

    public async Task CreateClient(Client client)
    {
        if (await _clientRepository.GetClientByIdAsync(client.Id) is not null)
        {
            throw new ClientExistsException(client.Id);
        }
        await _clientRepository.AddClientAsync(client);
    }

    public async Task<Client> UpdateClient(Client client)
    {
        if (await _clientRepository.GetClientByIdAsync(client.Id) is null)
        {
            throw new ClientNotFoundException(client.Id);
        }
        return await _clientRepository.UpdateClientAsync(client);
    }

    public async Task DeleteClient(Guid idClient)
    {
        if (await _clientRepository.GetClientByIdAsync(idClient) is null)
        {
            throw new ClientNotFoundException(idClient);
        }
        await _clientRepository.DeleteClientAsync(idClient);
    }
    
    public async Task<Client> GetClientById(Guid idClient)
    {
        return await _clientRepository.GetClientByIdAsync(idClient) ?? throw new ClientNotFoundException(idClient);
    }
    
    public async Task<List<Client>> GetClients(int? limit, int? offset)
    {
        return await _clientRepository.GetClientsAsync(limit, offset);
    }
    
    public async Task FreezeMembership(Guid idClient, DateOnly FreezingStart, int durationInDays)
    {
        var client = await  _clientRepository.GetClientByIdAsync(idClient);
        if (client is null)
        {
            throw new ClientNotFoundException(idClient);
        }

        if (durationInDays < MinFreezing)
        {
            throw new InvalidFreezingException("Duration should be at least 7 days.");
        }

        if (durationInDays > client.RemainFreezing)
        {
            throw new InvalidFreezingException("Duration exceeds remaining freezing days.");
        }

        var dateToCheck = new Tuple<DateOnly, DateOnly>(FreezingStart, FreezingStart.AddDays(durationInDays));
        if (client.FreezingIntervals.Any(interval => CheckOverlap(interval, dateToCheck)))
        {
            throw new InvalidFreezingException("Requested freezing period overlaps with existing intervals.");
        }

        client.RemainFreezing -= durationInDays;
        client.FreezingIntervals.Add(dateToCheck);
        client.MembershipEnd = client.MembershipEnd.AddDays(durationInDays);

        await _clientRepository.UpdateClientAsync(client);
    }

    private bool CheckOverlap(Tuple<DateOnly, DateOnly> existingInterval, Tuple<DateOnly, DateOnly> newInterval)
    {
        return (existingInterval.Item1 <= newInterval.Item1 && newInterval.Item1 <= existingInterval.Item2) ||
               (existingInterval.Item1 <= newInterval.Item2 && newInterval.Item2 <= existingInterval.Item2) ||
               (newInterval.Item1 <= existingInterval.Item1 && existingInterval.Item2 <= newInterval.Item2);
    }

    public async Task<DateOnly> GetMembershipEndDate(Guid idClient)
    {
        return await _clientRepository.GetMembershipEndDateAsync(idClient);
    }
    
    public async Task CheckAndSetFreezingStatus(Guid idClient)
    {
        var client = await _clientRepository.GetClientByIdAsync(idClient);
        if (client is null)
        {
            throw new ClientNotFoundException(idClient);
        }
        
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        bool isFrozenToday = client.FreezingIntervals.Any(interval => interval.Item1 <= today && today <= interval.Item2);
        if (client.IsFreezing != isFrozenToday)
        {
            client.IsFreezing = isFrozenToday;
            await _clientRepository.UpdateClientAsync(client);
        }
    }

}