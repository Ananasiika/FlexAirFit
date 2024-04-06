using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly FlexAirFitDbContext _context;

    public ProductRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(ProductConverter.CoreToDbModel(product));
        await _context.SaveChangesAsync();
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        var productDbModel = await _context.Products.FindAsync(product.Id);
        productDbModel.Type = product.Type;
        productDbModel.Name = product.Name;
        productDbModel.Price = product.Price;

        await _context.SaveChangesAsync();
        return ProductConverter.DbToCoreModel(productDbModel);
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var productDbModel = await _context.Products.FindAsync(id);
        _context.Products.Remove(productDbModel);
        await _context.SaveChangesAsync();
    }

    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        var productDbModel = await _context.Products.FindAsync(id);
        return ProductConverter.DbToCoreModel(productDbModel);
    }

    public async Task<List<Product>> GetProductsAsync(int? limit, int? offset = null)
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
        return productsDbModels.Select(ProductConverter.DbToCoreModel).ToList();
    }
}

