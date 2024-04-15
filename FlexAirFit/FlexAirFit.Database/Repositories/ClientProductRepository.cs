using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Repositories
{
    public class ClientProductRepository : IClientProductRepository
    {
        private readonly FlexAirFitDbContext _context;

        public ClientProductRepository(FlexAirFitDbContext context)
        {
            _context = context;
        }
        
        public async Task AddClientProductAsync(ClientProduct clientProduct)
        { 
            await _context.ClientProducts.AddAsync(ClientProductConverter.CoreToDbModel(clientProduct));
            await _context.SaveChangesAsync();
        }
    }
}