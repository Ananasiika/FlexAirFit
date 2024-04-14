using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Enums;
using FlexAirFit.Database.Repositories;
using IntegrationTests.DbFixtures;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Application.Services.Tests
{
    public class UserServiceTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService(new UserRepository(_dbContextFixture.Context));
        }

        [Fact]
        public async Task CreateUser_Should_Add_User_To_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var role = UserRole.Admin;
            var email = "admin@example.com";
            var password = "hashedpassword";

            var user = new User(id, role, email, password);

            // Act
            await _userService.CreateUser(user, password, role);

            // Assert
            var userDbModel = await _dbContextFixture.Context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.NotNull(userDbModel);
            Assert.Equal(user.Role, userDbModel.Role);
            Assert.Equal(user.Email, userDbModel.Email);
            Assert.Equal(user.PasswordHashed, userDbModel.PasswordHashed);
        }

        [Fact]
        public async Task SignInUser_Should_Return_User_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var role = UserRole.Admin;
            var email = "user@example.com";
            var password = "hashedpassword";

            var user = new User(id, role, email, password);
            await _userService.CreateUser(user, password, role);

            // Act
            var retrievedUser = await _userService.SignInUser(email, password);

            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Id, retrievedUser.Id);
            Assert.Equal(user.Role, retrievedUser.Role);
            Assert.Equal(user.Email, retrievedUser.Email);
            Assert.Equal(user.PasswordHashed, retrievedUser.PasswordHashed);
        }

        [Fact]
        public async Task GetUserById_Should_Return_User_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var role = UserRole.Admin;
            var email = "user@example.com";
            var password = "hashedpassword";

            var user = new User(id, role, email, password);
            await _userService.CreateUser(user, password, role);

            // Act
            var resultUser = await _userService.GetUserById(id);

            // Assert
            Assert.NotNull(resultUser);
            Assert.Equal(user.Id, resultUser.Id);
            Assert.Equal(user.Role, resultUser.Role);
            Assert.Equal(user.Email, resultUser.Email);
            Assert.Equal(user.PasswordHashed, resultUser.PasswordHashed);
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
