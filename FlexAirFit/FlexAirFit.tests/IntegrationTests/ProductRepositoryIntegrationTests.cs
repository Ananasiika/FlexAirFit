using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Repositories;
using IntegrationTests.DbFixtures;

namespace FlexAirFit.Application.Services.Tests
{
    public class ProductServiceTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _productService = new ProductService(new ProductRepository(_dbContextFixture.Context));
        }

        [Fact]
        public async Task CreateProduct_Should_Add_Product_To_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = ProductType.Solarium;
            var name = "Product Name";
            var price = 100;

            var product = new Product(id, type, name, price);

            // Act
            await _productService.CreateProduct(product);

            // Assert
            var productDbModel = await _dbContextFixture.Context.Products.FindAsync(product.Id);
            Assert.NotNull(productDbModel);
            Assert.Equal(product.Type, productDbModel.Type);
            Assert.Equal(product.Name, productDbModel.Name);
            Assert.Equal(product.Price, productDbModel.Price);
        }

        [Fact]
        public async Task UpdateProduct_Should_Update_Product_In_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = ProductType.Accessories;
            var name = "Product Name";
            var price = 100;

            var product = new Product(id, type, name, price);

            await _productService.CreateProduct(product);

            var newType = ProductType.Cloth;
            var newName = "New Product Name";
            var newPrice = 200;

            // Act
            await _productService.UpdateProduct(new Product(id, newType, newName, newPrice));

            // Assert
            var updatedProductDbModel = await _dbContextFixture.Context.Products.FindAsync(product.Id);
            Assert.NotNull(updatedProductDbModel);
            Assert.Equal(newType, updatedProductDbModel.Type);
            Assert.Equal(newName, updatedProductDbModel.Name);
            Assert.Equal(newPrice, updatedProductDbModel.Price);
        }

        [Fact]
        public async Task DeleteProduct_Should_Delete_Product_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = ProductType.Accessories;
            var name = "Product Name";
            var price = 100;

            var product = new Product(id, type, name, price);

            await _productService.CreateProduct(product);

            // Act
            await _productService.DeleteProduct(product.Id);

            // Assert
            var productDbModel = await _dbContextFixture.Context.Products.FindAsync(product.Id);
            Assert.Null(productDbModel);
        }

        [Fact]
        public async Task GetProductById_Should_Return_Product_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = ProductType.Solarium;
            var name = "Product Name";

            var price = 100;

            var product = new Product(id, type, name, price);

            await _productService.CreateProduct(product);

            // Act
            var result = await _productService.GetProductById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Type, result.Type);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
        }

        [Fact]
        public async Task GetProducts_Should_Return_List_Of_Products_From_Database()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product(Guid.NewGuid(), ProductType.Solarium, "Product 1", 200),
                new Product(Guid.NewGuid(), ProductType.Accessories, "Product 2", 150)
            };

            await _productService.CreateProduct(products[0]);
            await _productService.CreateProduct(products[1]);

            // Act
            var result = await _productService.GetProducts(null, null);

            // Assert
            Assert.Equal(products.Count, result.Count);
            for (int i = 0; i < products.Count; i++)
            {
                Assert.Equal(products[i].Type, result[i].Type);
                Assert.Equal(products[i].Name, result[i].Name);
                Assert.Equal(products[i].Price, result[i].Price);
            }
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
