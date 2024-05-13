using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlexAirFit.Database.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly FlexAirFitDbContext _context;
    private readonly ILogger _logger = Log.ForContext<AdminRepository>();

    public AdminRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddAdminAsync(Admin admin)
    {
        try
        {
            await _context.Admins.AddAsync(AdminConverter.CoreToDbModel(admin));
            await _context.SaveChangesAsync();
            _logger.Information($"Admin with ID {admin.Id} was successfully added.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while adding admin with ID {admin.Id}.");
        }
    }
    
    public async Task<Admin> UpdateAdminAsync(Admin admin)
    {
        try
        {
            var adminDbModel = await _context.Admins.FindAsync(admin.Id);
            adminDbModel.Name = admin.Name;
            adminDbModel.Gender = admin.Gender;
            adminDbModel.DateOfBirth = admin.DateOfBirth;
            
            await _context.SaveChangesAsync();
            _logger.Information($"Admin with ID {admin.Id} data updated successfully");
            return AdminConverter.DbToCoreModel(adminDbModel);
        }
        catch (Exception ex)
        {
            _logger.Error($"An error occurred while updating admin with ID {admin.Id} data");
            throw;
        }
    }

    public async Task DeleteAdminAsync(Guid id)
    {
        var admin = await _context.Admins.FindAsync(id);
        try
        {
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            _logger.Information($"Admin with ID {id} was successfully deleted.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An error occurred while deleting admin with ID {id}.");
        }
    }

    public async Task<Admin> GetAdminByIdAsync(Guid id)
    {
        var adminDbModel = await _context.Admins.FindAsync(id);
        return AdminConverter.DbToCoreModel(adminDbModel);
    }

    public async Task<List<Admin>> GetAdminsAsync(int? limit, int? offset)
    {
        try
        {
            var query = _context.Admins.AsQueryable();

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            var adminsDbModels = await query.ToListAsync();
            var admins = adminsDbModels.Select(AdminConverter.DbToCoreModel).ToList();

            _logger.Information($"Retrieved {admins.Count} admins" + (limit.HasValue ? $" with limit {limit.Value}" : "") + (offset.HasValue ? $" and offset {offset.Value}" : ""));
            return admins;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while retrieving admins.");
            throw;
        }
    }
}
