using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.Utils;
using FlexAirFit.Core.Enums;
using Serilog;
using Serilog.Context;

namespace FlexAirFit.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger _logger = Log.ForContext<UserService>();
    
    public async Task CreateUser(User user, string password, UserRole role)
    {
        User foundUser = await _userRepository.GetUserByEmailAsync(user.Email);
        if (foundUser is not null)
        {
            _logger.Warning($"User with email \"{user.Email}\" already registered. Skipping registration.");
            throw new UserRegisteredException($"User with email \"{user.Email}\" already registered");
        }
        user.PasswordHashed = PasswordHasher.HashPassword(password);
        user.Role = role;
        await _userRepository.AddUserAsync(user);
    }

    public async Task<User> UpdatePasswordUser(User user, string password)
    {
        if (await _userRepository.GetUserByIdAsync(user.Id) is null)
        {
            _logger.Error($"User with id \"{user.Id}\" not found");
            throw new UserNotFoundException(user.Id);
        }
        user.PasswordHashed = PasswordHasher.HashPassword(password);
        return await _userRepository.UpdateUserAsync(user);
    }
    
    public async Task<User> SignInUser(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user is null)
        {
            _logger.Warning($"User with email \"{email}\" not found. Skipping login.");
            throw new UserNotFoundException($"User with email \"{email}\" not found");
        }
            
        if (!PasswordHasher.VerifyPassword(password, user.PasswordHashed))
        {
            _logger.Warning($"Incorrect password for user with login \"{email}\"");
            throw new UserCredentialsException($"Incorrect password for user with login \"{email}\"");
        }
        _logger.Information($"User with login \"{email}\" successfully logged in");
        return user;
    }
    
    public async Task<User> GetUserById(Guid idUser)
    {
        return await _userRepository.GetUserByIdAsync(idUser) ?? throw new UserNotFoundException(idUser);
    }
    
    public async Task<List<User>> GetUsers(int? limit, int? offset)
    {
        return await _userRepository.GetUsersAsync(limit, offset);
    }
}