using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IRepositories;

public interface IAdminRepository
{
    Task AddAdminAsync(Admin adminException);
    Task<Admin> UpdateAdminAsync(Admin adminException);
    Task DeleteAdminAsync(Guid id);
    Task<Admin> GetAdminByIdAsync(Guid id);
    Task<List<Admin>> GetAdminsAsync(int? limit, int? offset);
}