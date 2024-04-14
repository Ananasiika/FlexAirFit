using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FlexAirFitDbContext _context;

    public UserRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(UserConverter.CoreToDbModel(user));
        await _context.SaveChangesAsync();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        var userDbModel = await _context.Users.FindAsync(user.Id);
        userDbModel.Role = user.Role;
        userDbModel.Email = user.Email;
        userDbModel.PasswordHashed = user.PasswordHashed;

        await _context.SaveChangesAsync();
        return UserConverter.DbToCoreModel(userDbModel);
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        var userDbModel = await _context.Users.FindAsync(id);
        return new User(userDbModel.Id, userDbModel.Role, userDbModel.Email, userDbModel.PasswordHashed);
    }

    public async Task<List<User>> GetUsersAsync(int? limit, int? offset)
    {
        var query = _context.Users.AsQueryable();

        if (offset.HasValue)
        {
            query = query.Skip(offset.Value);
        }

        if (limit.HasValue)
        {
            query = query.Take(limit.Value);
        }

        var userDbModels = await query.ToListAsync();
        return userDbModels.Select(x => new User(x.Id, x.Role, x.Email, x.PasswordHashed)).ToList();
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var userDbModel = await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());
        if (userDbModel == null)
            return null;
        return new User(userDbModel.Id, userDbModel.Role, userDbModel.Email, userDbModel.PasswordHashed);
    }
}
