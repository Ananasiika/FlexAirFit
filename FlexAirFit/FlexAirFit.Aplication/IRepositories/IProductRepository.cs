using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IRepositories;

public interface IProductRepository
{
    Task AddProductAsync(Product good);
    Task<Product> UpdateProductAsync(Product good);
    Task DeleteProductAsync(Guid id);
    Task<Product> GetProductByIdAsync(Guid id);
    Task<List<Product>> GetProductsAsync(int? limit, int? offset);
}