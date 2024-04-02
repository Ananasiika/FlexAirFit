using FlexAirFit.Core.Models;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Application.IServices;

public interface IUserService
{
    Task CreateUser(User user, string password, UserRole role);
    Task<User> UpdatePasswordUser(User user, string password);
    Task<User> SignInUser(string email, string password);
    Task<User> GetUserById(Guid id);
    Task<List<User>> GetUsers(int? limit, int? offset);
}