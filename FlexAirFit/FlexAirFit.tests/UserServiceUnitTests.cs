using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.Utils;
using FlexAirFit.Core.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlexAirFit.Application.Services;
using Xunit;

namespace FlexAirFit.Tests
{
    public class UserServiceUnitTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IUserService _userService;

        public UserServiceUnitTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task CreateUser_ShouldCallAddUserAsync_WhenUserDoesNotExist()
        {
            var user = new User(Guid.NewGuid(), UserRole.Client, "test@example.com", "hashedPassword");
            var password = "password";

            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(user.Email)).ReturnsAsync((User)null);

            await _userService.CreateUser(user, password, UserRole.Client);

            _mockUserRepository.Verify(r => r.AddUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task CreateUser_ShouldThrowUserRegisteredException_WhenUserExists()
        {
            var user = new User(Guid.NewGuid(), UserRole.Client, "test@example.com", "hashedPassword");
            var password = "password";

            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

            await Assert.ThrowsAsync<UserRegisteredException>(() => _userService.CreateUser(user, password, UserRole.Client));
        }

        [Fact]
        public async Task UpdatePasswordUser_ShouldCallUpdateUserAsync_WhenUserExists()
        {
            var user = new User(Guid.NewGuid(), UserRole.Client, "test@example.com", "hashedPassword");
            var password = "newPassword";

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(user.Id)).ReturnsAsync(user);

            await _userService.UpdatePasswordUser(user, password);

            _mockUserRepository.Verify(r => r.UpdateUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdatePasswordUser_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
        {
            var user = new User(Guid.NewGuid(), UserRole.Client, "test@example.com", "hashedPassword");
            var password = "newPassword";

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(user.Id)).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdatePasswordUser(user, password));
        }

        [Fact]
        public async Task SignInUser_ShouldReturnUser_WhenUserExistsAndPasswordIsCorrect()
        {
            var email = "test@example.com";
            var password = "password";
            var hashedPassword = PasswordHasher.HashPassword(password);
            var user = new User(Guid.NewGuid(), UserRole.Client, email, hashedPassword);

            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(user);

            var result = await _userService.SignInUser(email, password);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task SignInUser_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
        {
            var email = "test@example.com";
            var password = "password";

            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.SignInUser(email, password));
        }

        [Fact]
        public async Task SignInUser_ShouldThrowUserCredentialsException_WhenPasswordIsIncorrect()
        {
            var email = "test@example.com";
            var password = "password";
            var hashedPassword = PasswordHasher.HashPassword("differentPassword");
            var user = new User(Guid.NewGuid(), UserRole.Client, email, hashedPassword);

            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(user);

            await Assert.ThrowsAsync<UserCredentialsException>(() => _userService.SignInUser(email, password));
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var user = new User(userId, UserRole.Client, "test@example.com", "hashedPassword");

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var result = await _userService.GetUserById(userId);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetUserById_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserById(userId));
        }

        [Fact]
        public async Task GetUsers_ShouldReturnListOfUsers_WithLimitAndOffset()
        {
            var users = new List<User>
            {
                new User(Guid.NewGuid(), UserRole.Client, "user1@example.com", "hashedPassword1"),
                new User(Guid.NewGuid(), UserRole.Client, "user2@example.com", "hashedPassword2"),
                new User(Guid.NewGuid(), UserRole.Client, "user3@example.com", "hashedPassword3")
            };

            _mockUserRepository.Setup(r => r.GetUsersAsync(2, 1)).ReturnsAsync(users);

            var result = await _userService.GetUsers(2, 1);

            Assert.Equal(users, result);
        }
    }
}
