using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Serilog;

namespace FlexAirFit.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ILogger _logger = Log.ForContext<ProductService>();

    public async Task CreateProduct(Product good)
    {
        if (await _productRepository.GetProductByIdAsync(good.Id) is not null)
        {
            _logger.Warning($"Good with ID {good.Id} already exists in the database. Skipping creation.");
            throw new ProductExistsException(good.Id);
        }
        await _productRepository.AddProductAsync(good);
    }

    public async Task<Product> UpdateProduct(Product good)
    {
        if (await _productRepository.GetProductByIdAsync(good.Id) is null)
        {
            _logger.Warning($"Good with ID {good.Id} not found in the database. Skipping update.");
            throw new ProductNotFoundException(good.Id);
        }
        return await _productRepository.UpdateProductAsync(good);
    }

    public async Task DeleteProduct(Guid idProduct)
    {
        if (await _productRepository.GetProductByIdAsync(idProduct) is null)
        {
            _logger.Warning($"Good with ID {idProduct} not found in the database. Skipping deletion.");
            throw new ProductNotFoundException(idProduct);
        }
        await _productRepository.DeleteProductAsync(idProduct);
    }
    
    public async Task<Product> GetProductById(Guid idProduct)
    {
        var product = await _productRepository.GetProductByIdAsync(idProduct);
        if (product is null)
        {
            _logger.Warning($"Good with ID {idProduct} not found in the database.");
            throw new ProductNotFoundException(idProduct);
        }

        _logger.Information($"Good with ID {idProduct} was successfully retrieved.");
        return product;
    }
    
    public async Task<List<Product>> GetProducts(int? limit, int? offset)
    {
        return await _productRepository.GetProductsAsync(limit, offset);
    }
    
    public async Task<bool> CheckIfProductExists(Guid idProduct)
    {
        return !(await _productRepository.GetProductByIdAsync(idProduct) is null);
    }
}