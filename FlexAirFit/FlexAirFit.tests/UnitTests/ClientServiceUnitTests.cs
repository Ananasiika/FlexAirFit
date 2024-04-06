using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlexAirFit.Application.Services;
using Xunit;

namespace FlexAirFit.Tests;

public class ClientServiceUnitTests
{
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly IClientService _clientService;

    public ClientServiceUnitTests()
    {
        _mockClientRepository = new Mock<IClientRepository>();
        _clientService = new ClientService(_mockClientRepository.Object);
    }

    [Fact]
    public async Task CreateClient_ShouldCallAddClientAsync_WhenClientDoesNotExist()
    {
        var client = new Client(Guid.NewGuid(), Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>());

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(client.Id)).ReturnsAsync((Client)null);

        await _clientService.CreateClient(client);

        _mockClientRepository.Verify(r => r.AddClientAsync(client), Times.Once);
    }

    [Fact]
    public async Task CreateClient_ShouldThrowClientExistsException_WhenClientExists()
    {
        var client = new Client(Guid.NewGuid(), Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>());

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(client.Id)).ReturnsAsync(client);

        await Assert.ThrowsAsync<ClientExistsException>(() => _clientService.CreateClient(client));
    }

    [Fact]
    public async Task UpdateClient_ShouldCallUpdateClientAsync_WhenClientExists()
    {
        var client = new Client(Guid.NewGuid(), Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>());

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(client.Id)).ReturnsAsync(client);

        await _clientService.UpdateClient(client);

        _mockClientRepository.Verify(r => r.UpdateClientAsync(client), Times.Once);
    }

    [Fact]
    public async Task UpdateClient_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
    {
        var client = new Client(Guid.NewGuid(), Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>());

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(client.Id)).ReturnsAsync((Client)null);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientService.UpdateClient(client));
    }

    [Fact]
    public async Task DeleteClient_ShouldCallDeleteClientAsync_WhenClientExists()
    {
        var clientId = Guid.NewGuid();

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(new Client(clientId, Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>()));

        await _clientService.DeleteClient(clientId);

        _mockClientRepository.Verify(r => r.DeleteClientAsync(clientId), Times.Once);
    }

    [Fact]
    public async Task DeleteClient_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
    {
        var clientId = Guid.NewGuid();

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync((Client)null);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientService.DeleteClient(clientId));
    }

    [Fact]
    public async Task GetClientById_ShouldReturnClient_WhenClientExists()
    {
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>());

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        var result = await _clientService.GetClientById(clientId);

        Assert.Equal(client, result);
    }

    [Fact]
    public async Task GetClientById_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
    {
        var clientId = Guid.NewGuid();

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync((Client)null);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientService.GetClientById(clientId));
    }

    [Fact]
    public async Task GetClients_ShouldReturnListOfClients_WithLimitAndOffset()
    {
        var clients = new List<Client>
        {
            new Client(Guid.NewGuid(), Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>()),
            new Client(Guid.NewGuid(), Guid.NewGuid(), "Jane Smith", "Female", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 8, new List<Tuple<DateOnly, DateOnly>>()),
            new Client(Guid.NewGuid(), Guid.NewGuid(), "Alex Johnson", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 12, new List<Tuple<DateOnly, DateOnly>>())
        };

        _mockClientRepository.Setup(r => r.GetClientsAsync(2, 1)).ReturnsAsync(clients);

        var result = await _clientService.GetClients(2, 1);

        Assert.Equal(clients, result);
    }

    [Fact]
    public async Task FreezeMembership_ShouldCallUpdateClientAsync_WhenClientExistsAndFreezingIsValid()
    {
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>());
        var freezingStart = DateOnly.FromDateTime(DateTime.Now);
        var durationInDays = 7;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        await _clientService.FreezeMembership(clientId, freezingStart, durationInDays);

        _mockClientRepository.Verify(r => r.UpdateClientAsync(client), Times.Once);
    }

    [Fact]
    public async Task FreezeMembership_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
    {
        var clientId = Guid.NewGuid();
        var freezingStart = DateOnly.FromDateTime(DateTime.Now);
        var durationInDays = 7;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync((Client)null);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientService.FreezeMembership(clientId, freezingStart, durationInDays));
    }

    [Fact]
    public async Task FreezeMembership_ShouldThrowInvalidFreezingException_WhenDurationIsLessThanMinFreezing()
    {
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>());
        var freezingStart = DateOnly.FromDateTime(DateTime.Now);
        var durationInDays = 5;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        await Assert.ThrowsAsync<InvalidFreezingException>(() => _clientService.FreezeMembership(clientId, freezingStart, durationInDays));
    }

    [Fact]
    public async Task FreezeMembership_ShouldThrowInvalidFreezingException_WhenDurationExceedsRemainingFreezingDays()
    {
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 5, new List<Tuple<DateOnly, DateOnly>>());
        var freezingStart = DateOnly.FromDateTime(DateTime.Now);
        var durationInDays = 10;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        await Assert.ThrowsAsync<InvalidFreezingException>(() => _clientService.FreezeMembership(clientId, freezingStart, durationInDays));
    }

    [Fact]
    public async Task FreezeMembership_ShouldThrowInvalidFreezingException_WhenRequestedPeriodOverlapsWithExistingIntervals()
    {
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, Guid.NewGuid(), "John Doe", "Male", DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), 10, new List<Tuple<DateOnly, DateOnly>>
            {
                new Tuple<DateOnly, DateOnly>(DateOnly.FromDateTime(DateTime.Now).AddDays(2), DateOnly.FromDateTime(DateTime.Now).AddDays(8))
            });
        DateOnly freezingStart = DateOnly.FromDateTime(DateTime.Now).AddDays(5);
        var durationInDays = 5;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        await Assert.ThrowsAsync<InvalidFreezingException>(() => _clientService.FreezeMembership(clientId, freezingStart, durationInDays));
    }
}
