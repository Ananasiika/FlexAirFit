using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Models;
using IntegrationTests.DbFixtures;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Repositories.Tests
{
    public class ClientRepositoryTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IClientRepository _clientRepository;

        public ClientRepositoryTests()
        {
            _clientRepository = new ClientRepository(_dbContextFixture.Context);
        }

        [Fact]
        public async Task AddClientAsync_Should_Add_Client_To_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var idUser = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var dateOfBirth = new DateOnly(1985, 1, 1);
            var idMembership = Guid.NewGuid();
            var membershipEnd = new DateOnly(2023, 12, 31);
            var remainFreezing = (int?)null;
            var isFreezing = false;
            var freezingIntervals = new List<Tuple<DateOnly, DateOnly>>();

            var client = new Client(id, idUser, name, gender, dateOfBirth, idMembership, membershipEnd, remainFreezing, freezingIntervals);

            // Act
            await _clientRepository.AddClientAsync(client);

            // Assert
            var clientDbModel = await _dbContextFixture.Context.Clients.FindAsync(client.Id);
            Assert.NotNull(clientDbModel);
            Assert.Equal(client.IdUser, clientDbModel.IdUser);
            Assert.Equal(client.Name, clientDbModel.Name);
            Assert.Equal(client.Gender, clientDbModel.Gender);
            Assert.Equal(client.DateOfBirth, clientDbModel.DateOfBirth);
            Assert.Equal(client.IdMembership, clientDbModel.IdMembership);
            Assert.Equal(client.MembershipEnd, clientDbModel.MembershipEnd);
            Assert.Null(clientDbModel.RemainFreezing);
            Assert.False(clientDbModel.IsMembershipActive);
        }

        [Fact]
        public async Task UpdateClientAsync_Should_Update_Client_In_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var idUser = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var dateOfBirth = new DateOnly(1985, 1, 1);
            var idMembership = Guid.NewGuid();
            var membershipEnd = new DateOnly(2023, 12, 31);
            var remainFreezing = (int?)null;
            var isFreezing = false;
            var freezingIntervals = new List<Tuple<DateOnly, DateOnly>>();

            var client = new Client(id, idUser, name, gender, dateOfBirth, idMembership, membershipEnd, remainFreezing, freezingIntervals);

            await _dbContextFixture.Context.Clients.AddAsync(ClientConverter.CoreToDbModel(client));
            await _dbContextFixture.Context.SaveChangesAsync();

            var newName = "Jane Smith";
            var newMembershipEnd = new DateOnly(2024, 12, 31);

            // Act
            await _clientRepository.UpdateClientAsync(new Client(id, idUser, newName, gender, dateOfBirth, idMembership, newMembershipEnd, remainFreezing, freezingIntervals));

            // Assert

            var updatedClientDbModel = await _dbContextFixture.Context.Clients.FindAsync(client.Id);
            Assert.NotNull(updatedClientDbModel);
            Assert.Equal(newName, updatedClientDbModel.Name);
            Assert.Equal(newMembershipEnd, updatedClientDbModel.MembershipEnd);
        }

        [Fact]
        public async Task DeleteClientAsync_Should_Delete_Client_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var idUser = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var dateOfBirth = new DateOnly(1985, 1, 1);
            var idMembership = Guid.NewGuid();
            var membershipEnd = new DateOnly(2023, 12, 31);
            var remainFreezing = (int?)null;
            var isFreezing = false;
            var freezingIntervals = new List<Tuple<DateOnly, DateOnly>>();

            var client = new Client(id, idUser, name, gender, dateOfBirth, idMembership, membershipEnd, remainFreezing, freezingIntervals);

            await _dbContextFixture.Context.Clients.AddAsync(ClientConverter.CoreToDbModel(client));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            await _clientRepository.DeleteClientAsync(client.Id);

            // Assert
            var clientDbModel = await _dbContextFixture.Context.Clients.FindAsync(client.Id);
            Assert.Null(clientDbModel);
        }
        
        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
