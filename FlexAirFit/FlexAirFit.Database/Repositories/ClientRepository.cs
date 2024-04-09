using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly FlexAirFitDbContext _context;

    public ClientRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddClientAsync(Client client)
    {
        await _context.Clients.AddAsync(ClientConverter.CoreToDbModel(client));
        await _context.SaveChangesAsync();
    }

    public async Task<Client> UpdateClientAsync(Client client)
    {
        var clientDbModel = await _context.Clients.FindAsync(client.Id);
        clientDbModel.Name = client.Name;
        clientDbModel.Gender = client.Gender;
        clientDbModel.DateOfBirth = client.DateOfBirth;
        clientDbModel.MembershipEnd = client.MembershipEnd;
        clientDbModel.RemainFreezing = client.RemainFreezing;

        await _context.SaveChangesAsync();
        return ClientConverter.DbToCoreModel(clientDbModel);
    }

    public async Task DeleteClientAsync(Guid id)
    {
        var clientDbModel = await _context.Clients.FindAsync(id);
        _context.Clients.Remove(clientDbModel);
        await _context.SaveChangesAsync();
    }

    public async Task<Client> GetClientByIdAsync(Guid id)
    {
        var clientDbModel = await _context.Clients.FindAsync(id);
        return ClientConverter.DbToCoreModel(clientDbModel);
    }

    public async Task<List<Client>> GetClientsAsync(int? limit, int? offset = null)
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
        return clientsDbModels.Select(ClientConverter.DbToCoreModel).ToList();
    }

    public async Task<DateOnly> GetMembershipEndDateAsync(Guid idClient)
    {
        var clientDbModel = await _context.Clients.FindAsync(idClient);
        return clientDbModel.MembershipEnd;
    }
}

