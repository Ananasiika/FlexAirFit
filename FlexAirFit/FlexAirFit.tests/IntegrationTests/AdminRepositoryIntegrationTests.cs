using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FlexAirFit.Tests;
using IntegrationTests.DbFixtures;

namespace FlexAirFit.Database.Repositories.Tests
{
    public class AdminRepositoryTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IAdminRepository _adminRepository;

        public AdminRepositoryTests()
        {
            _adminRepository = new AdminRepository(_dbContextFixture.Context);
        }

        [Fact]
        public async Task AddAdminAsync_Should_Add_Admin_To_Database()
        {
            // Arrange
            var admin = new Admin(Guid.NewGuid(), Guid.NewGuid(), "John Doe", new DateTime(1990, 1, 1), "Male");

            // Act
            await _adminRepository.AddAdminAsync(admin);

            // Assert
            var adminDbModel = await _dbContextFixture.Context.Admins.FirstOrDefaultAsync(a => a.Id == admin.Id);
            Assert.NotNull(adminDbModel);
            Assert.Equal(admin.Name, adminDbModel.Name);
            Assert.Equal(admin.DateOfBirth, adminDbModel.DateOfBirth);
            Assert.Equal(admin.Gender, adminDbModel.Gender);
        }

        [Fact]
        public async Task DeleteAdminAsync_Should_Delete_Admin_From_Database()
        {
            // Arrange
            var admin = new Admin(Guid.NewGuid(), Guid.NewGuid(), "John Doe", new DateTime(1990, 1, 1), "Male");
            await _dbContextFixture.Context.Admins.AddAsync(AdminConverter.CoreToDbModel(admin));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            await _adminRepository.DeleteAdminAsync(admin.Id);

            // Assert
            var adminDbModel = await _dbContextFixture.Context.Admins.FirstOrDefaultAsync(a => a.Id == admin.Id);
            Assert.Null(adminDbModel);
        }

        [Fact]
        public async Task GetAdminByIdAsync_Should_Return_Admin_From_Database()
        {
            // Arrange
            var admin = new Admin(Guid.NewGuid(), Guid.NewGuid(), "John Doe", new DateTime(1990, 1, 1), "Male");
            await _dbContextFixture.Context.Admins.AddAsync(AdminConverter.CoreToDbModel(admin));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _adminRepository.GetAdminByIdAsync(admin.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(admin.Name, result.Name);
            Assert.Equal(admin.DateOfBirth, result.DateOfBirth);

            Assert.Equal(admin.Gender, result.Gender);
        }

        [Fact]
        public async Task GetAdminsAsync_Should_Return_List_Of_Admins_From_Database()
        {
            // Arrange
            var admins = new List<Admin>
            {
                new Admin(Guid.NewGuid(), Guid.NewGuid(), "John Doe", new DateTime(1990, 1, 1), "Male"),
                new Admin(Guid.NewGuid(), Guid.NewGuid(), "Jane Smith", new DateTime(1995, 2, 1), "Female")
            };

            await _dbContextFixture.Context.Admins.AddRangeAsync(admins.Select(AdminConverter.CoreToDbModel));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _adminRepository.GetAdminsAsync(null, null);

            // Assert
            Assert.Equal(admins.Count, result.Count);
            for (int i = 0; i < admins.Count; i++)
            {
                Assert.Equal(admins[i].Name, result[i].Name);
                Assert.Equal(admins[i].DateOfBirth, result[i].DateOfBirth);
                Assert.Equal(admins[i].Gender, result[i].Gender);
            }
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
