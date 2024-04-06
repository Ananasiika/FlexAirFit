using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Models;
using IntegrationTests.DbFixtures;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Repositories.Tests
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            _productRepository = new ProductRepository(_dbContextFixture.Context);
        }

        [Fact]
        public async Task AddProductAsync_Should_Add_Product_To_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = ProductType.Solarium;
            var name = "Product Name";
            var price = 100;

            var product = new Product(id, type, name, price);

            // Act
            await _productRepository.AddProductAsync(product);

            // Assert
            var productDbModel = await _dbContextFixture.Context.Products.FindAsync(product.Id);
            Assert.NotNull(productDbModel);
            Assert.Equal(product.Type, productDbModel.Type);
            Assert.Equal(product.Name, productDbModel.Name);
            Assert.Equal(product.Price, productDbModel.Price);
        }

        [Fact]
        public async Task UpdateProductAsync_Should_Update_Product_In_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = ProductType.Accessories;
            var name = "Product Name";
            var price = 100;

            var product = new Product(id, type, name, price);

            await _dbContextFixture.Context.Products.AddAsync(ProductConverter.CoreToDbModel(product));
            await _dbContextFixture.Context.SaveChangesAsync();

            var newType = ProductType.Cloth;
            var newName = "New Product Name";
            var newPrice = 200;

            // Act
            await _productRepository.UpdateProductAsync(new Product(id, newType, newName, newPrice));

            // Assert
            var updatedProductDbModel = await _dbContextFixture.Context.Products.FindAsync(product.Id);
            Assert.NotNull(updatedProductDbModel);
            Assert.Equal(newType, updatedProductDbModel.Type);
            Assert.Equal(newName, updatedProductDbModel.Name);
            Assert.Equal(newPrice, updatedProductDbModel.Price);
        }

        [Fact]
        public async Task DeleteProductAsync_Should_Delete_Product_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = ProductType.Accessories;
            var name = "Product Name";
            var price = 100;

            var product = new Product(id, type, name, price);

            await _dbContextFixture.Context.Products.AddAsync(ProductConverter.CoreToDbModel(product));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            await _productRepository.DeleteProductAsync(product.Id);

            // Assert
            var productDbModel = await _dbContextFixture.Context.Products.FindAsync(product.Id);
            Assert.Null(productDbModel);
        }

        [Fact]
        public async Task GetProductByIdAsync_Should_Return_Product_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();

            var type = ProductType.FoodProduct;
            var name = "Product Name";
            var price = 100;

            var product = new Product(id, type, name, price);

            await _dbContextFixture.Context.Products.AddAsync(ProductConverter.CoreToDbModel(product));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProductByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Type, result.Type);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
