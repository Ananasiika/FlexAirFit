using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace FlexAirFit.Application.Services;

public class ClientService(IClientRepository clientRepository,
                           IMembershipRepository membershipRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IMembershipRepository _membershipRepository = membershipRepository;
    private readonly ILogger _logger = Log.ForContext<ClientService>();

    public async Task CreateClient(Client client)
    {
        if (await _clientRepository.GetClientByIdAsync(client.Id) is not null)
        {
            _logger.Warning($"Client with ID {client.Id} already exists in the database. Skipping addition of Client.");
            throw new ClientExistsException(client.Id);
        }
        await _clientRepository.AddClientAsync(client);
    }

    public async Task<Client> UpdateClient(Client client)
    {
        if (await _clientRepository.GetClientByIdAsync(client.Id) is null)
        {
            _logger.Warning($"Client with ID {client.Id} does not exist in the database. Skipping update of Client.");
            throw new ClientNotFoundException(client.Id);
        }
        return await _clientRepository.UpdateClientAsync(client);
    }

    public async Task DeleteClient(Guid idClient)
    {
        if (await _clientRepository.GetClientByIdAsync(idClient) is null)
        {
            _logger.Warning($"Client with ID {idClient} does not exist in the database. Skipping deletion of Client.");
            throw new ClientNotFoundException(idClient);
        }
        await _clientRepository.DeleteClientAsync(idClient);
    }
    
    public async Task<Client> GetClientById(Guid idClient)
    {
        var client = await _clientRepository.GetClientByIdAsync(idClient);
        if (client is null)
        {
            _logger.Warning($"Client with ID {idClient} does not exist in the database.");
            throw new ClientNotFoundException(idClient);
        }

        _logger.Information($"Client with ID {idClient} was successfully retrieved.");
        return client;
    }
    
    public async Task<List<Client>> GetClients(int? limit, int? offset)
    {
        return await _clientRepository.GetClientsAsync(limit, offset);
    }
    
    public async Task FreezeMembership(Guid idClient, DateTime FreezingStart, int durationInDays)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json") 
            .Build();
        
        var client = await  _clientRepository.GetClientByIdAsync(idClient);
        if (client is null)
        {
            _logger.Warning($"Client with ID {idClient} does not exist in the database. Skipping freezing of membership.");
            throw new ClientNotFoundException(idClient);
        }

        if (durationInDays < int.Parse(configuration["MinFreezing"]))
        {
            _logger.Warning($"Duration should be at least {configuration["MinFreezing"]} days. Skipping freezing of membership.");
            throw new InvalidFreezingException("Duration should be at least " + configuration["MinFreezing"] + " days.");
        }

        if (durationInDays > client.RemainFreezing)
        {
            _logger.Warning($"Duration exceeds remaining freezing days. Skipping freezing of membership.");
            throw new InvalidFreezingException("Duration exceeds remaining freezing days.");
        }
        
        if (DateTime.Today > FreezingStart)
        {
            _logger.Warning($"The start date of freezing must be no earlier than today. Skipping freezing of membership.");
            throw new InvalidFreezingException("The start date of freezing must be no earlier than today.");
        }

        DateTime?[] dateToCheck = { FreezingStart, FreezingStart.AddDays(durationInDays) };
        if (client.FreezingIntervals != null && client.FreezingIntervals.Length != 0)
        {
            if (client.FreezingIntervals.Any(interval => CheckOverlap(interval, dateToCheck)))
            {
                _logger.Warning($"Requested freezing period overlaps with existing intervals. Skipping freezing of membership.");
                throw new InvalidFreezingException("Requested freezing period overlaps with existing intervals.");
            } 
        }

        client.RemainFreezing -= durationInDays;

        if (client.FreezingIntervals == null || client.FreezingIntervals.Length == 0)
        {
            client.FreezingIntervals = new DateTime?[][] { dateToCheck };
        }
        else
        {
            DateTime?[][] newIntervals = new DateTime?[client.FreezingIntervals.Length + 1][];
            Array.Copy(client.FreezingIntervals, newIntervals, client.FreezingIntervals.Length);
            newIntervals[newIntervals.Length - 1] = dateToCheck;
            client.FreezingIntervals = newIntervals;
        }

        client.MembershipEnd = client.MembershipEnd.AddDays(durationInDays);
        await _clientRepository.UpdateClientAsync(client);
        _logger.Information($"Client with ID {idClient} was successfully frozen for {durationInDays} days.");
    }

    private bool CheckOverlap(DateTime?[] existingInterval, DateTime?[] newInterval)
    {
        return (existingInterval[0] < newInterval[0] && newInterval[0] < existingInterval[1]) ||
               (existingInterval[0] < newInterval[1] && newInterval[1] < existingInterval[1]) ||
               (newInterval[0] <= existingInterval[0] && existingInterval[1] <= newInterval[1]);
    }

    public async Task<DateTime> GetMembershipEndDate(Guid idClient)
    {
        return await _clientRepository.GetMembershipEndDateAsync(idClient);
    }
    
    public async Task<bool> CheckIfClientExists(Guid idClient)
    {
        return !(await _clientRepository.GetClientByIdAsync(idClient) is null);
    }
    
    public async Task AddMembership(Guid idClient, Guid idMembership)
    {
        var client = await  _clientRepository.GetClientByIdAsync(idClient);
        if (client is null)
        {
            _logger.Warning($"Client with ID {idClient} does not exist in the database. Skipping adding of membership.");
            throw new ClientNotFoundException(idClient);
        }
        
        var membership = await  _membershipRepository.GetMembershipByIdAsync(idMembership);
        if (membership is null)
        {
            _logger.Warning($"Membership with ID {idMembership} does not exist in the database. Skipping adding of membership.");
            throw new MembershipNotFoundException(idMembership);
        }

        client.MembershipEnd.AddDays(membership.Duration.Days);
        client.IdMembership = idMembership;
        client.RemainFreezing += membership.Freezing;

        await _clientRepository.UpdateClientAsync(client);
    }
}