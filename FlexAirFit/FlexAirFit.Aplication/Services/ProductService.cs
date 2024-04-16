using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;

namespace FlexAirFit.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task CreateProduct(Product good)
    {
        if (await _productRepository.GetProductByIdAsync(good.Id) is not null)
        {
            throw new ProductExistsException(good.Id);
        }
        await _productRepository.AddProductAsync(good);
    }

    public async Task<Product> UpdateProduct(Product good)
    {
        if (await _productRepository.GetProductByIdAsync(good.Id) is null)
        {
            throw new ProductNotFoundException(good.Id);
        }
        return await _productRepository.UpdateProductAsync(good);
    }

    public async Task DeleteProduct(Guid idProduct)
    {
        if (await _productRepository.GetProductByIdAsync(idProduct) is null)
        {
            throw new ProductNotFoundException(idProduct);
        }
        await _productRepository.DeleteProductAsync(idProduct);
    }
    
    public async Task<Product> GetProductById(Guid idProduct)
    {
        return await _productRepository.GetProductByIdAsync(idProduct) ?? throw new ProductNotFoundException(idProduct);
    }
    
    public async Task<List<Product>> GetProducts(int? limit, int? offset)
    {
        return await _productRepository.GetProductsAsync(limit, offset);
    }
    
    public async Task<bool> CheckIfProductExists(Guid idProduct)
    {
        if (await _productRepository.GetProductByIdAsync(idProduct) is null)
        {
            return false;
        }
        return true;
    }
}