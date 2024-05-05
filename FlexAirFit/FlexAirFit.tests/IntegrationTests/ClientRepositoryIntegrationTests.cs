using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Models;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Database.Repositories;
using IntegrationTests.DbFixtures;

namespace FlexAirFit.Application.Services.Tests
{
    public class ClientServiceTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IClientService _clientService;

        public ClientServiceTests()
        {
            _clientService = new ClientService(new ClientRepository(_dbContextFixture.Context),
                                               new MembershipRepository(_dbContextFixture.Context));
        }

        [Fact]
        public async Task CreateClient_Should_Add_Client_To_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var idUser = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var dateOfBirth = new DateTime(1985, 1, 1);
            var idMembership = Guid.NewGuid();
            var membershipEnd = new DateTime(2023, 12, 31);
            var remainFreezing = (int?)null;

            var client = new Client(id, name, gender, dateOfBirth, idMembership, membershipEnd, remainFreezing, null);

            // Act
            await _clientService.CreateClient(client);

            // Assert
            var clientDbModel = await _dbContextFixture.Context.Clients.FindAsync(client.Id);
            Assert.NotNull(clientDbModel);
            Assert.Equal(client.Name, clientDbModel.Name);
            Assert.Equal(client.Gender, clientDbModel.Gender);
            Assert.Equal(client.DateOfBirth, clientDbModel.DateOfBirth);
            Assert.Equal(client.IdMembership, clientDbModel.IdMembership);
            Assert.Equal(client.MembershipEnd, clientDbModel.MembershipEnd);
            Assert.Null(clientDbModel.RemainFreezing);
            Assert.False(clientDbModel.IsMembershipActive);
        }

        [Fact]
        public async Task UpdateClient_Should_Update_Client_In_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var idUser = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var dateOfBirth = new DateTime(1985, 1, 1);
            var idMembership = Guid.NewGuid();
            var membershipEnd = new DateTime(2023, 12, 31);
            var remainFreezing = (int?)null;

            var client = new Client(id, name, gender, dateOfBirth, idMembership, membershipEnd, remainFreezing, null);

            await _clientService.CreateClient(client);

            client.Name = "Jane Smith";
            client.MembershipEnd = new DateTime(2024, 12, 31);

            // Act
            await _clientService.UpdateClient(client);

            // Assert
            var updatedClient = await _dbContextFixture.Context.Clients.FindAsync(id);
            Assert.NotNull(updatedClient);
            Assert.Equal(client.Name, updatedClient.Name);
            Assert.Equal(client.MembershipEnd, updatedClient.MembershipEnd);
        }

        [Fact]
        public async Task DeleteClient_Should_Delete_Client_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var idUser = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var dateOfBirth = new DateTime(1985, 1, 1);
            var idMembership = Guid.NewGuid();
            var membershipEnd = new DateTime(2023, 12, 31);
            var remainFreezing = (int?)null;

            var client = new Client(id, name, gender, dateOfBirth, idMembership, membershipEnd, remainFreezing, null);

            await _clientService.CreateClient(client);

            // Act
            await _clientService.DeleteClient(id);

            // Assert
            var deletedClient = await _dbContextFixture.Context.Clients.FindAsync(id);
            Assert.Null(deletedClient);
        }

        [Fact]
        public async Task GetClientById_Should_Return_Client_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var idUser = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var dateOfBirth = new DateTime(1985, 1, 1);
            var idMembership = Guid.NewGuid();
            var membershipEnd = new DateTime(2023, 12, 31);
            var remainFreezing = (int?)null;

            var client = new Client(id, name, gender, dateOfBirth, idMembership, membershipEnd, remainFreezing, null);

            await _clientService.CreateClient(client);

            // Act
            var result = await _clientService.GetClientById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(client.Name, result.Name);
            Assert.Equal(client.Gender, result.Gender);
            Assert.Equal(client.DateOfBirth, result.DateOfBirth);
        }

        [Fact]
        public async Task GetClients_Should_Return_List_Of_Clients_From_Database()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client(Guid.NewGuid(), "John Doe", "Male", new DateTime(1985, 1, 1), Guid.NewGuid(), new DateTime(2023, 12, 31), null, null),
                new Client(Guid.NewGuid(), "Jane Smith", "Female", new DateTime(1990, 5, 15), Guid.NewGuid(), new DateTime(2022, 6, 30), null, null)
            };

            await _clientService.CreateClient(clients[0]);
            await _clientService.CreateClient(clients[1]);

            // Act
            var result = await _clientService.GetClients(null, null);

            // Assert
            Assert.Equal(clients.Count, result.Count);
            for (int i = 0; i < clients.Count; i++)
            {
                Assert.Equal(clients[i].Name, result[i].Name);
                Assert.Equal(clients[i].Gender, result[i].Gender);
                Assert.Equal(clients[i].DateOfBirth, result[i].DateOfBirth);
            }
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
