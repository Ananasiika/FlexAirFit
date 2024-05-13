using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlexAirFit.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FlexAirFitDbContext _context;
    private readonly ILogger _logger = Log.ForContext<UserRepository>();

    public UserRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(User user)
    {
        try
        {
            await _context.Users.AddAsync(UserConverter.CoreToDbModel(user));
            await _context.SaveChangesAsync();
            _logger.Information($"User with ID {user.Id} was successfully added.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while adding user with ID {user.Id}.");
        }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            var userDbModel = await _context.Users.FindAsync(user.Id);

            userDbModel.Role = user.Role;
            userDbModel.Email = user.Email;
            userDbModel.PasswordHashed = user.PasswordHashed;

            await _context.SaveChangesAsync();
            _logger.Information($"User with ID {user.Id} was successfully updated.");
            return UserConverter.DbToCoreModel(userDbModel);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while updating user with ID {user.Id}.");
            throw;
        }
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        var userDbModel = await _context.Users.FindAsync(id);
        return UserConverter.DbToCoreModel(userDbModel);
    }

    public async Task<List<User>> GetUsersAsync(int? limit, int? offset)
    {
        try
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
            var users = userDbModels.Select(x => new User(x.Id, x.Role, x.Email, x.PasswordHashed)).ToList();

            _logger.Information($"Retrieved {users.Count} users" + (limit.HasValue ? $" with limit {limit.Value}" : "") + (offset.HasValue ? $" and offset {offset.Value}" : ""));

            return users;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving users.");
            throw;
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var userDbModel = await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());
        return UserConverter.DbToCoreModel(userDbModel);
    }
}
