using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlexAirFit.Application.Services;
using FlexAirFit.Core.Enums;
using Xunit;

namespace FlexAirFit.Tests;

public class ProductServiceUnitTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly IProductService _productService;

    public ProductServiceUnitTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _productService = new ProductService(_mockProductRepository.Object);
    }

    [Fact]
    public async Task CreateProduct_ShouldCallAddProductAsync_WhenProductDoesNotExist()
    {
        var product = new Product(Guid.NewGuid(), ProductType.Accessories, "Sample Product", 100);

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(product.Id)).ReturnsAsync((Product)null);

        await _productService.CreateProduct(product);

        _mockProductRepository.Verify(r => r.AddProductAsync(product), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_ShouldThrowProductExistsException_WhenProductExists()
    {
        var product = new Product(Guid.NewGuid(), ProductType.Accessories, "Sample Product", 100);

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(product.Id)).ReturnsAsync(product);

        await Assert.ThrowsAsync<ProductExistsException>(() => _productService.CreateProduct(product));
    }

    [Fact]
    public async Task UpdateProduct_ShouldCallUpdateProductAsync_WhenProductExists()
    {
        var product = new Product(Guid.NewGuid(), ProductType.Cloth, "Sample Product", 100);

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(product.Id)).ReturnsAsync(product);

        await _productService.UpdateProduct(product);

        _mockProductRepository.Verify(r => r.UpdateProductAsync(product), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
    {
        var product = new Product(Guid.NewGuid(), ProductType.Membership, "Sample Product", 100);

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(product.Id)).ReturnsAsync((Product)null);

        await Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.UpdateProduct(product));
    }

    [Fact]
    public async Task DeleteProduct_ShouldCallDeleteProductAsync_WhenProductExists()
    {
        var productId = Guid.NewGuid();

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(new Product(productId, ProductType.Cloth, "Sample Product", 100));

        await _productService.DeleteProduct(productId);

        _mockProductRepository.Verify(r => r.DeleteProductAsync(productId), Times.Once);
    }

    [Fact]
    public async Task DeleteProduct_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
    {
        var productId = Guid.NewGuid();

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync((Product)null);

        await Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.DeleteProduct(productId));
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
    {
        var productId = Guid.NewGuid();

        var product = new Product(productId, ProductType.Membership, "Sample Product", 100);

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);

        var result = await _productService.GetProductById(productId);

        Assert.Equal(product, result);
    }

    [Fact]
    public async Task GetProductById_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
    {
        var productId = Guid.NewGuid();

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync((Product)null);

        await Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.GetProductById(productId));
    }

    [Fact]
    public async Task GetProducts_ShouldReturnListOfProducts_WithLimitAndOffset()
    {
        var products = new List<Product>
        {
            new Product(Guid.NewGuid(), ProductType.Accessories, "Sample Product 1", 100),
            new Product(Guid.NewGuid(), ProductType.Cloth, "Sample Product 2", 200),
            new Product(Guid.NewGuid(), ProductType.Solarium, "Sample Product 3", 300)
        };

        _mockProductRepository.Setup(r => r.GetProductsAsync(2, 1)).ReturnsAsync(products);

        var result = await _productService.GetProducts(2, 1);

        Assert.Equal(products, result);
    }
}

