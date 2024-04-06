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
using Xunit;
using FlexAirFit.Tests;
using IntegrationTests.DbFixtures;

namespace FlexAirFit.Database.Repositories.Tests;

public class UserRepositoryTests : IDisposable
{
    private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
    private readonly IUserRepository _userRepository;

    public UserRepositoryTests()
    {
        _userRepository = new UserRepository(_dbContextFixture.Context);
    }
    
    [Fact]
    public async Task TestAddUserAsync()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), UserRole.Admin, "admin@example.com", "hashedpassword");

        // Act
        await _userRepository.AddUserAsync(user);

        // Assert
        var userDbModel = await _dbContextFixture.Context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(userDbModel);
        Assert.Equal(user.Role, userDbModel.Role);
        Assert.Equal(user.Email, userDbModel.Email);
        Assert.Equal(user.PasswordHashed, userDbModel.PasswordHashed);
    }

    [Fact]
    public async Task TestUpdateUserAsync()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), UserRole.Admin, "user@example.com", "hashedpassword");
        await _dbContextFixture.Context.Users.AddAsync(UserConverter.CoreToDbModel(user));
        await _dbContextFixture.Context.SaveChangesAsync();

        // Act
        user.Email = "updateduser@example.com";

        await _userRepository.UpdateUserAsync(user);

        // Assert
        var userDbModel = await _dbContextFixture.Context.Users.FindAsync(user.Id);
        Assert.Equal(user.Email, userDbModel.Email);
    }

    [Fact]
    public async Task TestGetUserByIdAsync()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), UserRole.Admin, "user@example.com", "hashedpassword");
        await _dbContextFixture.Context.Users.AddAsync(UserConverter.CoreToDbModel(user));
        await _dbContextFixture.Context.SaveChangesAsync();

        // Act
        var resultUser = await _userRepository.GetUserByIdAsync(user.Id);

        // Assert
        Assert.Equal(user.Id, resultUser.Id);
        Assert.Equal(user.Role, resultUser.Role);
        Assert.Equal(user.Email, resultUser.Email);
        Assert.Equal(user.PasswordHashed, resultUser.PasswordHashed);
    }

    [Fact]
    public async Task TestGetUsersAsync()
    {
        // Arrange
        var users = new List<User>
        {
            new User(Guid.NewGuid(), UserRole.Admin, "user1@example.com", "hashedpassword1"),
            new User(Guid.NewGuid(), UserRole.Admin, "user2@example.com", "hashedpassword2"),
            new User(Guid.NewGuid(), UserRole.Admin, "user3@example.com", "hashedpassword3"),
        };

        await _dbContextFixture.Context.Users.AddRangeAsync(users.Select(UserConverter.CoreToDbModel));
        await _dbContextFixture.Context.SaveChangesAsync();

        // Act
        var resultUsers = await _userRepository.GetUsersAsync(null, null);

        // Assert

        Assert.Equal(users.Count, resultUsers.Count);
        foreach (var (expectedUser, actualUser) in users.Zip(resultUsers, Tuple.Create))
        {
            Assert.Equal(expectedUser.Id, actualUser.Id);
            Assert.Equal(expectedUser.Role, actualUser.Role);
            Assert.Equal(expectedUser.Email, actualUser.Email);
            Assert.Equal(expectedUser.PasswordHashed, actualUser.PasswordHashed);
        }
    }

    [Fact]
    public async Task TestGetUserByEmailAsync_ExistingUser()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), UserRole.Admin, "user@example.com", "hashedpassword");
        await _dbContextFixture.Context.Users.AddAsync(UserConverter.CoreToDbModel(user));
        await _dbContextFixture.Context.SaveChangesAsync();

        // Act
        var resultUser = await _userRepository.GetUserByEmailAsync(user.Email);

        // Assert
        Assert.Equal(user.Id, resultUser.Id);
        Assert.Equal(user.Role, resultUser.Role);
        Assert.Equal(user.Email, resultUser.Email);
        Assert.Equal(user.PasswordHashed, resultUser.PasswordHashed);
    }

    [Fact]
    public async Task TestGetUserByEmailAsync_NonExistingUser()
    {
        // Arrange
        const string nonExistingEmail = "nonexistinguser@example.com";

        // Act
        var resultUser = await _userRepository.GetUserByEmailAsync(nonExistingEmail);

        // Assert
        Assert.Null(resultUser);
    }
    

    public void Dispose()
    {
        _dbContextFixture.Dispose();
    }
}

