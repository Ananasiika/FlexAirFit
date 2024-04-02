using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;

namespace FlexAirFit.Application.Services;

public class AdminService(IAdminRepository adminRepository) : IAdminService
{
    private readonly IAdminRepository _adminRepository = adminRepository;

    public async Task CreateAdmin(Admin admin)
    {
        if (await _adminRepository.GetAdminByIdAsync(admin.Id) is not null)
        {
            throw new AdminExistsException(admin.Id);
        }
        await _adminRepository.AddAdminAsync(admin);
    }

    public async Task<Admin> UpdateAdmin(Admin admin)
    {
        if (await _adminRepository.GetAdminByIdAsync(admin.Id) is null)
        {
            throw new AdminNotFoundException(admin.Id);
        }
        return await _adminRepository.UpdateAdminAsync(admin);
    }

    public async Task DeleteAdmin(Guid idAdmin)
    {
        if (await _adminRepository.GetAdminByIdAsync(idAdmin) is null)
        {
            throw new AdminNotFoundException(idAdmin);
        }
        await _adminRepository.DeleteAdminAsync(idAdmin);
    }
}