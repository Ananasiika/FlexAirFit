using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Services;
using FlexAirFit.Core.Enums;
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

    public ClientProductServiceUnitTests()
    {
        _mockClientProductRepository = new Mock<IClientProductRepository>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockBonusRepository = new Mock<IBonusRepository>();
        _mockClientRepository = new Mock<IClientRepository>();
        _clientProductService = new ClientProductService(_mockClientProductRepository.Object, _mockProductRepository.Object, _mockBonusRepository.Object, _mockClientRepository.Object);
    }

    [Fact]
    public async Task AddClientProductAndReturnCost_ShouldUpdateClientBonusCountAndReturnTotalCost()
    {
        var clientProduct = new ClientProduct(Guid.NewGuid(), Guid.NewGuid());

        var product = new Product(Guid.NewGuid(), ProductType.PersonalWorkout, "Product 1", 100);

        var bonusCount = 50;
        var totalCost = 100;

        _mockProductRepository.Setup(p => p.GetProductByIdAsync(clientProduct.IdProduct)).ReturnsAsync(product);
        _mockBonusRepository.Setup(b => b.GetCountBonusByIdClientAsync(clientProduct.IdClient)).ReturnsAsync(bonusCount);
        _mockBonusRepository.Setup(b => b.UpdateCountBonusByIdClientAsync(clientProduct.IdClient, bonusCount));

        var result = await _clientProductService.AddClientProductAndReturnCost(clientProduct, false);

        Assert.Equal(totalCost, result);
    }

    [Fact]
    public async Task AddClientProductAndReturnCost_ShouldUpdateClientBonusCountByMaxBonusCountIfWriteOffIsOne()
    {
        var clientProduct = new ClientProduct(Guid.NewGuid(), Guid.NewGuid());

        var product = new Product(Guid.NewGuid(), ProductType.Cloth, "Product 2", 50);

        var bonusCount = 30;
        var totalCost = 25;

        _mockProductRepository.Setup(p => p.GetProductByIdAsync(clientProduct.IdProduct)).ReturnsAsync(product);
        _mockBonusRepository.Setup(b => b.GetCountBonusByIdClientAsync(clientProduct.IdClient)).ReturnsAsync(bonusCount);
        _mockBonusRepository.Setup(b => b.UpdateCountBonusByIdClientAsync(clientProduct.IdClient, bonusCount));

        var result = await _clientProductService.AddClientProductAndReturnCost(clientProduct, true);

        Assert.Equal(totalCost, result);
    }

    [Fact]
    public async Task AddClientProductAndReturnCost_ShouldNotUpdateClientBonusCountIfWriteOffIsZero()
    {
        var clientProduct = new ClientProduct(Guid.NewGuid(), Guid.NewGuid());

        var product = new Product(Guid.NewGuid(), ProductType.Accessories, "Product 3", 75);

        var bonusCount = 40;
        var totalCost = 75;

        _mockProductRepository.Setup(p => p.GetProductByIdAsync(clientProduct.IdProduct)).ReturnsAsync(product);
        _mockBonusRepository.Setup(b => b.GetCountBonusByIdClientAsync(clientProduct.IdClient)).ReturnsAsync(bonusCount);
        _mockBonusRepository.Setup(b => b.UpdateCountBonusByIdClientAsync(clientProduct.IdClient, bonusCount));

        var result = await _clientProductService.AddClientProductAndReturnCost(clientProduct, false);

        Assert.Equal(totalCost, result);
    }
    
    [Fact]
    public async Task AddClientProductAndReturnCost_CountBonusesLessHalfCost()
    {
        var clientProduct = new ClientProduct(Guid.NewGuid(), Guid.NewGuid());

        var product = new Product(Guid.NewGuid(), ProductType.FoodProduct, "Product 3", 80);

        var bonusCount = 30;
        var totalCost = 50;

        _mockProductRepository.Setup(p => p.GetProductByIdAsync(clientProduct.IdProduct)).ReturnsAsync(product);
        _mockBonusRepository.Setup(b => b.GetCountBonusByIdClientAsync(clientProduct.IdClient)).ReturnsAsync(bonusCount);
        _mockBonusRepository.Setup(b => b.UpdateCountBonusByIdClientAsync(clientProduct.IdClient, bonusCount));

        var result = await _clientProductService.AddClientProductAndReturnCost(clientProduct, true);

        Assert.Equal(totalCost, result);
    }
}
