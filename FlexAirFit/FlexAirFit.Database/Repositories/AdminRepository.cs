using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly FlexAirFitDbContext _context;

    public AdminRepository(FlexAirFitDbContext context)
    {
        _context = context;
    }

    public async Task AddAdminAsync(Admin admin)
    {
        await _context.Admins.AddAsync(AdminConverter.CoreToDbModel(admin));
        await _context.SaveChangesAsync();
    }

    public async Task<Admin> UpdateAdminAsync(Admin admin)
    {
        var adminDbModel = await _context.Admins.FindAsync(admin.Id);
        adminDbModel.Name = admin.Name;
        adminDbModel.Gender = admin.Gender;
        adminDbModel.DateOfBirth = admin.DateOfBirth;
        
        await _context.SaveChangesAsync();
        return AdminConverter.DbToCoreModel(adminDbModel);
    }

    public async Task DeleteAdminAsync(Guid id)
    {
        var admin = await _context.Admins.FindAsync(id);
        if (admin != null)
        {
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Admin> GetAdminByIdAsync(Guid id)
    {
        var adminDbModel = await _context.Admins.FindAsync(id);
        return AdminConverter.DbToCoreModel(adminDbModel);
    }

    public async Task<List<Admin>> GetAdminsAsync(int? limit, int? offset)
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

        var adminsDbModels =  await query.ToListAsync();
        return adminsDbModels.Select(AdminConverter.DbToCoreModel).ToList();
    }
}
