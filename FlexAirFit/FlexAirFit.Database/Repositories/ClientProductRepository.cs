using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Converters;
using Serilog;

namespace FlexAirFit.Database.Repositories
{
    public class ClientProductRepository : IClientProductRepository
    {
        private readonly FlexAirFitDbContext _context;
        private readonly ILogger _logger = Log.ForContext<ClientProductRepository>();

        public ClientProductRepository(FlexAirFitDbContext context)
        {
            _context = context;
        }

        public async Task AddClientProductAsync(ClientProduct clientProduct)
        {
            try
            {
                await _context.ClientProducts.AddAsync(ClientProductConverter.CoreToDbModel(clientProduct));
                await _context.SaveChangesAsync();
                _logger.Information($"ClientProduct with IDs Client: {clientProduct.IdClient}, Product: {clientProduct.IdProduct} was successfully added.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while adding ClientProduct with IDs Client: {clientProduct.IdClient}, Product: {clientProduct.IdProduct}.");
            }
        }
    }
}