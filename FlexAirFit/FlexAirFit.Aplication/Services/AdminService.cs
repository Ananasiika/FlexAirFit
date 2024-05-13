using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Serilog;


namespace FlexAirFit.Application.Services;

public class AdminService(IAdminRepository adminRepository) : IAdminService
{
    private readonly IAdminRepository _adminRepository = adminRepository;
    private readonly ILogger _logger = Log.ForContext<AdminService>();

    public async Task CreateAdmin(Admin admin)
    {
        if (await _adminRepository.GetAdminByIdAsync(admin.Id) is not null)
        {
            _logger.Warning($"Admin with ID {admin.Id} already exists in the database. Skipping creation.");
            throw new AdminExistsException(admin.Id);
        }
        await _adminRepository.AddAdminAsync(admin);
    }

    public async Task<Admin> UpdateAdmin(Admin admin)
    {
        if (await _adminRepository.GetAdminByIdAsync(admin.Id) is null)
        {
            _logger.Warning($"Admin with ID {admin.Id} not found in the database. Skipping update.");
            throw new AdminNotFoundException(admin.Id);
        }
        return await _adminRepository.UpdateAdminAsync(admin);
    }
    
    public async Task DeleteAdmin(Guid idAdmin)
    {
        if (await _adminRepository.GetAdminByIdAsync(idAdmin) is null)
        {
            _logger.Warning($"Admin with ID {idAdmin} not found in the database. Skipping deletion.");
            throw new AdminNotFoundException(idAdmin);
        }
        await _adminRepository.DeleteAdminAsync(idAdmin);
    }
}