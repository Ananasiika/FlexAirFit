using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Services;
using FlexAirFit.Core.Enums;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace FlexAirFit.Tests;
public class ClientProductServiceUnitTests
{
    private readonly Mock<IClientProductRepository> _mockClientProductRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IBonusRepository> _mockBonusRepository;
    private readonly IClientProductService _clientProductService;
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly IBonusService _bonusService;

    public ClientProductServiceUnitTests()
    {
        _mockClientProductRepository = new Mock<IClientProductRepository>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockBonusRepository = new Mock<IBonusRepository>();
        _mockClientRepository = new Mock<IClientRepository>();
        _bonusService = new BonusService(_mockBonusRepository.Object);
        _clientProductService = new ClientProductService(_mockClientProductRepository.Object, _mockProductRepository.Object, _mockBonusRepository.Object, _mockClientRepository.Object);
    }
    
    [Fact]
    public async Task AddClientProductAndReturnCost_ShouldUpdateClientBonusCountAndReturnTotalCost()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today.AddDays(10), 200, null);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        var productId = Guid.NewGuid();
        var product = new Product(productId, ProductType.PersonalWorkout, "Workout", 1000);
        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);

        var clientProduct = new ClientProduct(clientId, productId);

        var bonus = new Bonus(Guid.NewGuid(), clientId, 200);
        _mockBonusRepository.Setup(r => r.GetCountBonusByIdClientAsync(clientId)).ReturnsAsync(bonus.Count);

        var expectedTotalCost = 800;
        var expectedBonusCount = 180; 

        // Act
        var result = await _clientProductService.AddClientProductAndReturnCost(clientProduct, true);

        // Assert
        _mockClientProductRepository.Verify(r => r.AddClientProductAsync(It.IsAny<ClientProduct>()), Times.Once());
        _mockBonusRepository.Verify(r => r.UpdateCountBonusByIdClientAsync(clientId, expectedBonusCount), Times.Once());
        
        Assert.Equal(expectedTotalCost, result);
    }
    
    [Fact]
    public async Task AddClientProductAndReturnCost_WithoutBonusRedemption_ShouldNotDeductBonusesAndReturnTotalCost()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today.AddDays(10), 200, null);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        var productId = Guid.NewGuid();
        var product = new Product(productId, ProductType.Cloth, "Cloth", 500);
        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);

        var clientProduct = new ClientProduct(clientId, productId);

        var bonus = new Bonus(Guid.NewGuid(), clientId, 200);
        _mockBonusRepository.Setup(r => r.GetCountBonusByIdClientAsync(clientId)).ReturnsAsync(bonus.Count);

        var expectedTotalCost = 500;
        var expectedBonusCount = 250;

        // Act
        var result = await _clientProductService.AddClientProductAndReturnCost(clientProduct, false);

        // Assert
        _mockClientProductRepository.Verify(r => r.AddClientProductAsync(It.IsAny<ClientProduct>()), Times.Once());
        _mockBonusRepository.Verify(r => r.UpdateCountBonusByIdClientAsync(clientId, expectedBonusCount), Times.Once());
        
        Assert.Equal(expectedTotalCost, result);
    }
    
    [Fact]
    public async Task AddClientProductAndReturnCost_WithoutBonusRedemption_ShouldNotDeductBonusesAndReturnTotalCost_Workout()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today.AddDays(10), 200, null);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        var productId = Guid.NewGuid();
        var product = new Product(productId, ProductType.PersonalWorkout, "Work", 500);
        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);

        var clientProduct = new ClientProduct(clientId, productId);

        var bonus = new Bonus(Guid.NewGuid(), clientId, 200);
        _mockBonusRepository.Setup(r => r.GetCountBonusByIdClientAsync(clientId)).ReturnsAsync(bonus.Count);

        var expectedTotalCost = 500;
        var expectedBonusCount = 350;

        // Act
        var result = await _clientProductService.AddClientProductAndReturnCost(clientProduct, false);

        // Assert
        _mockClientProductRepository.Verify(r => r.AddClientProductAsync(It.IsAny<ClientProduct>()), Times.Once());
        _mockBonusRepository.Verify(r => r.UpdateCountBonusByIdClientAsync(clientId, expectedBonusCount), Times.Once());
        
        Assert.Equal(expectedTotalCost, result);
    }

    [Fact]
    public async Task AddClientProductAndReturnCost_WithBonusRedemption_ShouldDeductBonusesAndReturnTotalCost()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today.AddDays(10), 200, null);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        var productId = Guid.NewGuid();
        var product = new Product(productId, ProductType.PersonalWorkout, "Workout", 1000);
        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);

        var clientProduct = new ClientProduct(clientId, productId);

        var bonus = new Bonus(Guid.NewGuid(), clientId, 300);
        _mockBonusRepository.Setup(r => r.GetCountBonusByIdClientAsync(clientId)).ReturnsAsync(bonus.Count);

        var expectedTotalCost = 700;
        var expectedBonusCount = 170;

        // Act
        var result = await _clientProductService.AddClientProductAndReturnCost(clientProduct, true);

        // Assert
        _mockClientProductRepository.Verify(r => r.AddClientProductAsync(It.IsAny<ClientProduct>()), Times.Once());
        _mockBonusRepository.Verify(r => r.UpdateCountBonusByIdClientAsync(clientId, expectedBonusCount), Times.Once());

        Assert.Equal(expectedTotalCost, result);
    }
    
    [Fact]
    public async Task AddClientProductAndReturnCost_WithBonusRedemptionLimitReached_ShouldDeductMaximumAllowedBonusesAndReturnTotalCost()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today.AddDays(10), 2000, null);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        var productId = Guid.NewGuid();
        var product = new Product(productId, ProductType.PersonalWorkout, "Workout", 1000);
        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);

        var clientProduct = new ClientProduct(clientId, productId);

        var bonus = new Bonus(Guid.NewGuid(), clientId, 2000);
        _mockBonusRepository.Setup(r => r.GetCountBonusByIdClientAsync(clientId)).ReturnsAsync(bonus.Count);

        var expectedTotalCost = 500;
        var expectedBonusCount = 1650;
        
        // Act
        var result = await _clientProductService.AddClientProductAndReturnCost(clientProduct, true);

        // Assert
        _mockClientProductRepository.Verify(r => r.AddClientProductAsync(It.IsAny<ClientProduct>()), Times.Once());
        _mockBonusRepository.Verify(r => r.UpdateCountBonusByIdClientAsync(clientId, expectedBonusCount), Times.Once());

        Assert.Equal(expectedTotalCost, result);
    }

    [Fact]
    public async Task AddClientProductAndReturnCost_WithMultiplePurchases_ShouldUpdateBonusCountCorrectly()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client(clientId, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today.AddDays(10), 200, null);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        var product1Id = Guid.NewGuid();
        var product1 = new Product(product1Id, ProductType.PersonalWorkout, "Workout", 1000);
        _mockProductRepository.Setup(r => r.GetProductByIdAsync(product1Id)).ReturnsAsync(product1);

        var product2Id = Guid.NewGuid();
        var product2 = new Product(product2Id, ProductType.Cloth, "Cloth", 500);
        _mockProductRepository.Setup(r => r.GetProductByIdAsync(product2Id)).ReturnsAsync(product2);

        var clientProduct1 = new ClientProduct(clientId, product1Id);
        var clientProduct2 = new ClientProduct(clientId, product2Id);

        var bonus = new Bonus(Guid.NewGuid(), clientId, 200);
        _mockBonusRepository.Setup(r => r.GetCountBonusByIdClientAsync(clientId)).ReturnsAsync(bonus.Count);

        var expectedTotalCost1 = 800;
        var expectedBonusCount1 = 180; 
        
        var expectedTotalCost2 = 300;
        var expectedBonusCount2 = 30; 

        // Act
        var result1 = await _clientProductService.AddClientProductAndReturnCost(clientProduct1, true);
        var result2 = await _clientProductService.AddClientProductAndReturnCost(clientProduct2, true);

        Assert.Equal(expectedTotalCost1, result1);
        Assert.Equal(expectedTotalCost2, result2);
        // Assert
        _mockClientProductRepository.Verify(r => r.AddClientProductAsync(It.IsAny<ClientProduct>()), Times.Exactly(2));
        _mockBonusRepository.Verify(r => r.UpdateCountBonusByIdClientAsync(clientId, expectedBonusCount1), Times.Once());
        _mockBonusRepository.Verify(r => r.UpdateCountBonusByIdClientAsync(clientId, expectedBonusCount2), Times.Once());

    }
}