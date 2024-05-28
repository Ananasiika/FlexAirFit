using System.Text.Json;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlexAirFit.Database.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly FlexAirFitDbContext _context;
    private readonly ILogger _logger = Log.ForContext<ClientRepository>();

    public ClientRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddClientAsync(Client client)
    {
        try
        {
            await _context.Clients.AddAsync(ClientConverter.CoreToDbModel(client));
            await _context.SaveChangesAsync();
            _logger.Information($"Client with ID {client.Id} was successfully added.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while adding client with ID {client.Id}.");
        }
    }

    public async Task<Client> UpdateClientAsync(Client client)
    {
        try
        {
            var clientDbModel = await _context.Clients.FindAsync(client.Id);
            clientDbModel.MembershipEnd = client.MembershipEnd;
            if (client.FreezingIntervals != null)
            {
                clientDbModel.FreezingIntervals = JsonDocument.Parse(
                    JsonSerializer.Serialize(
                        client.FreezingIntervals.Select(interval => new
                        {
                            start_date = interval[0]?.ToString("yyyy-MM-dd"),
                            end_date = interval[1]?.ToString("yyyy-MM-dd")
                        })));
            }
            clientDbModel.IdMembership = client.IdMembership;
            clientDbModel.RemainFreezing = client.RemainFreezing;
            clientDbModel.Gender = client.Gender;
            clientDbModel.DateOfBirth = client.DateOfBirth;
            clientDbModel.Name = client.Name;

            await _context.SaveChangesAsync();
            _logger.Information($"Client with ID {client.Id} was successfully updated.");
            return ClientConverter.DbToCoreModel(clientDbModel);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while updating client with ID {client.Id}.");
            throw;
        }
    }

    public async Task DeleteClientAsync(Guid id)
    {
        try
        {
            var clientDbModel = await _context.Clients.FindAsync(id);
            if (clientDbModel != null)
            {
                _context.Clients.Remove(clientDbModel);
                await _context.SaveChangesAsync();
                _logger.Information($"Client with ID {id} was successfully deleted.");
            }
            else
            {
                _logger.Error($"Client with ID {id} not found in the database.");
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while deleting client with ID {id}.");
        }
    }

    public async Task<Client> GetClientByIdAsync(Guid id)
    {
        var clientDbModel = await _context.Clients.FindAsync(id);
        return ClientConverter.DbToCoreModel(clientDbModel);
    }

    public async Task<List<Client>> GetClientsAsync(int? limit, int? offset = null)
    {
        try
        {
            var query = _context.Clients.AsQueryable();

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var clientsDbModels = await query.ToListAsync();
            var clients = clientsDbModels.Select(ClientConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {clients.Count} clients" + (limit.HasValue ? $" with limit {limit.Value}" : "") + (offset.HasValue ? $" and offset {offset.Value}" : ""));

            return clients;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving clients.");
            throw;
        }
    }

    public async Task<DateTime> GetMembershipEndDateAsync(Guid idClient)
    {
        try
        {
            var clientDbModel = await _context.Clients.FindAsync(idClient);
            if (clientDbModel == null)
            {
                _logger.Error($"Client with ID {idClient} not found in the database.");
                return default;
            }

            var membershipEndDate = clientDbModel.MembershipEnd;
            _logger.Information($"Membership end date {membershipEndDate} for client with ID {idClient} was successfully retrieved.");
            return membershipEndDate;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while retrieving membership end date for client with ID {idClient}.");
            throw;
        }
    }
}
