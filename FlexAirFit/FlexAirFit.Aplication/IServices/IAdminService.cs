using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IServices;

public interface IAdminService
{
    Task CreateAdmin(Admin adminException);
    Task<Admin> UpdateAdmin(Admin adminException);
    Task DeleteAdmin(Guid id);
}
