using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.Services;
using Client = FlexAirFit.Core.Models.Client;

namespace FlexAirFit.Tests;

public class ClientServiceUnitTests
{
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly Mock<IMembershipRepository> _mockMembershipRepository;
    private readonly IClientService _clientService;
    private readonly MembershipService _membershipService;

    public ClientServiceUnitTests()
    {
        _mockClientRepository = new Mock<IClientRepository>();
        _mockMembershipRepository = new Mock<IMembershipRepository>();
        _membershipService = new MembershipService(_mockMembershipRepository.Object);
        _clientService = new ClientService(_mockClientRepository.Object, _mockMembershipRepository.Object);
    }

    [Fact]
    public async Task CreateClient_ShouldCallAddClientAsync_WhenClientDoesNotExist()
    {
        var client = new Client(Guid.NewGuid(), "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(client.Id)).ReturnsAsync((Client)null);

        await _clientService.CreateClient(client);

        _mockClientRepository.Verify(r => r.AddClientAsync(client), Times.Once);
    }

    [Fact]
    public async Task CreateClient_ShouldThrowClientExistsException_WhenClientExists()
    {
        var client = new Client(Guid.NewGuid(), "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(client.Id)).ReturnsAsync(client);

        await Assert.ThrowsAsync<ClientExistsException>(() => _clientService.CreateClient(client));
    }

    [Fact]
    public async Task UpdateClient_ShouldCallUpdateClientAsync_WhenClientExists()
    {
        var client = new Client(Guid.NewGuid(), "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(client.Id)).ReturnsAsync(client);

        await _clientService.UpdateClient(client);

        _mockClientRepository.Verify(r => r.UpdateClientAsync(client), Times.Once);
    }

    [Fact]
    public async Task UpdateClient_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
    {
        var client = new Client(Guid.NewGuid(), "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(client.Id)).ReturnsAsync((Client)null);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientService.UpdateClient(client));
    }

    [Fact]
    public async Task DeleteClient_ShouldCallDeleteClientAsync_WhenClientExists()
    {
        var clientId = Guid.NewGuid();

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null));

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
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);

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
            new Client(Guid.NewGuid(), "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null),
            new Client(Guid.NewGuid(), "Jane Smith", "Female", DateTime.Today, Guid.NewGuid(), DateTime.Today, 8, null),
            new Client(Guid.NewGuid(), "Alex Johnson", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 12, null)
        };

        _mockClientRepository.Setup(r => r.GetClientsAsync(2, 1)).ReturnsAsync(clients);

        var result = await _clientService.GetClients(2, 1);

        Assert.Equal(clients, result);
    }

    [Fact]
    public async Task FreezeMembership_ShouldCallUpdateClientAsync_WhenClientExistsAndFreezingIsValid()
    {
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);
        var freezingStart = DateTime.Today;
        var durationInDays = 7;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        await _clientService.FreezeMembership(clientId, freezingStart, durationInDays);
       
        _mockClientRepository.Verify(r => r.UpdateClientAsync(client), Times.Once);
    }
    
    [Fact]
    public async Task FreezeMembership_ShouldCallUpdateClientAsync_WhenClientExistsAndMultipleNonOverlappingFreezingsAreValid()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today.AddDays(10), 20, null);
        var freezingStart1 = DateTime.Today;
        var durationInDays1 = 7;
        var freezingStart2 = freezingStart1.AddDays(durationInDays1 + 1); // Непересекающаяся вторая заморозка
        var durationInDays2 = 7;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        // Act
        await _clientService.FreezeMembership(clientId, freezingStart1, durationInDays1);
        await _clientService.FreezeMembership(clientId, freezingStart2, durationInDays2);

        // Assert
        _mockClientRepository.Verify(r => r.UpdateClientAsync(It.IsAny<Client>()), Times.Exactly(2));

        // Дополнительные проверки
        Assert.Equal(2, client.FreezingIntervals.Length);
        Assert.Equal(new DateTime?[] { freezingStart1, freezingStart1.AddDays(durationInDays1) }, client.FreezingIntervals[0]);
        Assert.Equal(new DateTime?[] { freezingStart2, freezingStart2.AddDays(durationInDays2) }, client.FreezingIntervals[1]);
        Assert.Equal(client.RemainFreezing, 20 - durationInDays1 - durationInDays2);
        Assert.Equal(client.MembershipEnd, DateTime.Today.AddDays(10 + durationInDays1 + durationInDays2));
    }
    
    [Fact]
    public async Task FreezeMembership_ShouldThrowException_WhenClientExistsAndMultipleFreezingsOverlap()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today.AddDays(10), 20, null);
        var freezingStart1 = DateTime.Today;
        var durationInDays1 = 7;
        
        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        // Act
        await _clientService.FreezeMembership(clientId, freezingStart1, durationInDays1);
        await Assert.ThrowsAsync<InvalidFreezingException>(() => _clientService.FreezeMembership(clientId, freezingStart1.AddDays(2), durationInDays1));

        // Assert
        _mockClientRepository.Verify(r => r.UpdateClientAsync(It.IsAny<Client>()), Times.Once());

        // Дополнительные проверки
        Assert.Single(client.FreezingIntervals);
        Assert.Equal(new DateTime?[] { freezingStart1, freezingStart1.AddDays(durationInDays1) }, client.FreezingIntervals[0]);
        Assert.Equal(client.RemainFreezing, 20 - durationInDays1);
        Assert.Equal(client.MembershipEnd, DateTime.Today.AddDays(10 + durationInDays1));
    }
    
    [Fact]
    public async Task FreezeMembership_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
    {
        var clientId = Guid.NewGuid();
        var freezingStart = DateTime.Today;
        var durationInDays = 7;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync((Client)null);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientService.FreezeMembership(clientId, freezingStart, durationInDays));
    }

    [Fact]
    public async Task FreezeMembership_ShouldThrowInvalidFreezingException_WhenDurationIsLessThanMinFreezing()
    {
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);
        var freezingStart = DateTime.Today;
        var durationInDays = 5;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        await Assert.ThrowsAsync<InvalidFreezingException>(() => _clientService.FreezeMembership(clientId, freezingStart, durationInDays));
    }

    [Fact]
    public async Task FreezeMembership_ShouldThrowInvalidFreezingException_WhenDurationExceedsRemainingFreezingDays()
    {
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 5, null);
        var freezingStart = DateTime.Today;
        var durationInDays = 10;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        await Assert.ThrowsAsync<InvalidFreezingException>(() => _clientService.FreezeMembership(clientId, freezingStart, durationInDays));
    }

    [Fact]
    public async Task FreezeMembership_ShouldThrowInvalidFreezingException_WhenRequestedPeriodOverlapsWithExistingIntervals()
    {
        var clientId = Guid.NewGuid();
        DateTime?[] dateIntervals = { DateTime.Today, DateTime.Today.AddDays(2) };

        DateTime?[][] intervals = new []{dateIntervals};
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, intervals);
        DateTime freezingStart = DateTime.Today.AddDays(5);
        var durationInDays = 5;

        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        await Assert.ThrowsAsync<InvalidFreezingException>(() => _clientService.FreezeMembership(clientId, freezingStart, durationInDays));
    }
}

