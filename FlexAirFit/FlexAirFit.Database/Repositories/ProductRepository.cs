using System.Diagnostics.CodeAnalysis;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlexAirFit.Database.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly FlexAirFitDbContext _context;
    private readonly ILogger _logger = Log.ForContext<ProductRepository>();

    public ProductRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddProductAsync(Product product)
    {
        try
        {
            await _context.Products.AddAsync(ProductConverter.CoreToDbModel(product));
            await _context.SaveChangesAsync();
            _logger.Information($"Product with ID {product.Id} was successfully added.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while adding product with ID {product.Id}.");
        }
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        try
        {
            var productDbModel = await _context.Products.FindAsync(product.Id);
            productDbModel.Type = product.Type;
            productDbModel.Name = product.Name;
            productDbModel.Price = product.Price;

            await _context.SaveChangesAsync();
            _logger.Information($"Product with ID {product.Id} was successfully updated.");
            return ProductConverter.DbToCoreModel(productDbModel);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while updating product with ID {product.Id}.");
            throw;
        }
    }

    public async Task DeleteProductAsync(Guid id)
    {
        try
        {
            var productDbModel = await _context.Products.FindAsync(id);
            if (productDbModel != null)
            {
                _context.Products.Remove(productDbModel);
                await _context.SaveChangesAsync();
                _logger.Information($"Product with ID {id} was successfully deleted.");
            }
            else
            {
                _logger.Error($"Product with ID {id} not found in the database.");
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while deleting product with ID {id}.");
        }
    }

    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        var productDbModel = await _context.Products.FindAsync(id);
        return ProductConverter.DbToCoreModel(productDbModel);
    }

    public async Task<List<Product>> GetProductsAsync(int? limit, int? offset = null)
    {
        try
        {
            var query = _context.Products.AsQueryable();

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var productsDbModels = await query.ToListAsync();
            var products = productsDbModels.Select(ProductConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {products.Count} products" + (limit.HasValue ? $" with limit {limit.Value}" : "") + (offset.HasValue ? $" and offset {offset.Value}" : ""));

            return products;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while retrieving products.");
            throw;
        }
    }
}
