using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Microsoft.Extensions.Configuration;

namespace FlexAirFit.Application.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

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

    public async Task<Client> GetClientByIdUser(Guid id)
    {
        return await _clientRepository.GetClientByIdUserAsync(id) ?? throw new ClientUserNotFoundException(id);
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
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json") 
            .Build();
        
        var client = await  _clientRepository.GetClientByIdAsync(idClient);
        if (client is null)
        {
            throw new ClientNotFoundException(idClient);
        }

        if (durationInDays < int.Parse(configuration["MinFreezing"]))
        {
            throw new InvalidFreezingException("Duration should be at least 7 days.");
        }

        if (durationInDays > client.RemainFreezing)
        {
            throw new InvalidFreezingException("Duration exceeds remaining freezing days.");
        }
        
        if (DateOnly.FromDateTime(DateTime.Today) > FreezingStart)
        {
            throw new InvalidFreezingException("The start date of freezing must be no earlier than today.");
        }
        
        var dateToCheck = new Tuple<DateOnly, DateOnly>(FreezingStart, FreezingStart.AddDays(durationInDays));
        if (client.FreezingIntervals != null)
        {
            if (client.FreezingIntervals.Any(interval => CheckOverlap(interval, dateToCheck)))
            {
                throw new InvalidFreezingException("Requested freezing period overlaps with existing intervals.");
            } 
        }
        
        client.RemainFreezing -= durationInDays;
        if (client.FreezingIntervals == null)
        {
            client.FreezingIntervals = new List<Tuple<DateOnly, DateOnly>>();
        }
        client.MembershipEnd = client.MembershipEnd.AddDays(durationInDays);
        await _clientRepository.UpdateClientAsync(client);
    }

    private bool CheckOverlap(Tuple<DateOnly, DateOnly> existingInterval, Tuple<DateOnly, DateOnly> newInterval)
    {
        if (existingInterval == null)
            return false;
        return (existingInterval.Item1 <= newInterval.Item1 && newInterval.Item1 <= existingInterval.Item2) ||
               (existingInterval.Item1 <= newInterval.Item2 && newInterval.Item2 <= existingInterval.Item2) ||
               (newInterval.Item1 <= existingInterval.Item1 && existingInterval.Item2 <= newInterval.Item2);
    }

    public async Task<DateOnly> GetMembershipEndDate(Guid idClient)
    {
        return await _clientRepository.GetMembershipEndDateAsync(idClient);
    }
    
    public async Task<bool> CheckIfClientExists(Guid idClient)
    {
        if (await _clientRepository.GetClientByIdAsync(idClient) is null)
        {
            return false;
        }
        return true;
    }
    
}