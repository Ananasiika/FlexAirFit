using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IServices;

public interface IProductService
{
    Task CreateProduct(Product good);
    Task<Product> UpdateProduct(Product good);
    Task DeleteProduct(Guid id);
    Task<Product> GetProductById(Guid id);
    Task<List<Product>> GetProducts(int? limit, int? offset);
}