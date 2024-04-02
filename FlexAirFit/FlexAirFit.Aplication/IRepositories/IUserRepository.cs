using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IRepositories;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<User> GetUserByIdAsync(Guid id);
    Task<List<User>> GetUsersAsync(int? limit, int? offset);
    Task<User> GetUserByEmailAsync(string email);
}