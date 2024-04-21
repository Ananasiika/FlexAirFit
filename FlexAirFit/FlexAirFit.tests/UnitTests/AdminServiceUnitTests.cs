using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.Services;

namespace FlexAirFit.Tests;

public class AdminServiceUnitTests : IAdminService
{
    private readonly Mock<IAdminRepository> _mockAdminRepository;
    private readonly IAdminService _adminService;

    public AdminServiceUnitTests()
    {
        _mockAdminRepository = new Mock<IAdminRepository>();
        _adminService = new AdminService(_mockAdminRepository.Object);
    }
    
    [Fact]
    public async Task CreateAdmin_ShouldAddNewAdmin_WhenAdminDoesNotExist()
    {
        var adminId = Guid.NewGuid();
        var admin = new Admin(adminId, "John Doe", DateTime.Now, "Male");

        _mockAdminRepository
            .Setup(r => r.GetAdminByIdAsync(adminId))
            .ReturnsAsync((Admin)null);
        _mockAdminRepository
            .Setup(r => r.AddAdminAsync(admin))
            .Returns(Task.CompletedTask);

        await _adminService.CreateAdmin(admin);

        _mockAdminRepository.Verify(r => r.AddAdminAsync(admin), Times.Once);
    }

    [Fact]
    public async Task CreateAdmin_ShouldThrowException_WhenAdminAlreadyExists()
    {
        var adminId = Guid.NewGuid();
        var admin = new Admin(adminId, "Jane Smith", DateTime.Now, "Female");

        _mockAdminRepository
            .Setup(r => r.GetAdminByIdAsync(adminId))
            .ReturnsAsync(admin);

        await Assert.ThrowsAsync<AdminExistsException>(() => _adminService.CreateAdmin(admin));
    }

    [Fact]
    public async Task UpdateAdmin_ShouldUpdateExistingAdmin_WhenAdminExists()
    {
        var adminId = Guid.NewGuid();
        var admin = new Admin(adminId, "John Doe", DateTime.Now, "Male");

        _mockAdminRepository
            .Setup(r => r.GetAdminByIdAsync(adminId))
            .ReturnsAsync(admin);
        _mockAdminRepository
            .Setup(r => r.UpdateAdminAsync(admin))
            .ReturnsAsync(admin);

        var updatedAdmin = await _adminService.UpdateAdmin(admin);

        _mockAdminRepository.Verify(r => r.UpdateAdminAsync(admin), Times.Once);
        
        Assert.Equal(admin, updatedAdmin);
    }

    [Fact]
    public async Task UpdateAdmin_ShouldThrowException_WhenAdminDoesNotExist()
    {
        var adminId = Guid.NewGuid();
        var admin = new Admin(adminId, "Jane Smith", DateTime.Now, "Female");

        _mockAdminRepository
            .Setup(r => r.GetAdminByIdAsync(adminId))
            .ReturnsAsync((Admin)null);

        await Assert.ThrowsAsync<AdminNotFoundException>(() => _adminService.UpdateAdmin(admin));
    }

    [Fact]
    public async Task DeleteAdmin_ShouldDeleteExistingAdmin_WhenAdminExists()
    {
        var adminId = Guid.NewGuid();
        var admin = new Admin(adminId, "John Doe", DateTime.Now, "Male");

        _mockAdminRepository
            .Setup(r => r.GetAdminByIdAsync(adminId))
            .ReturnsAsync(admin);
        _mockAdminRepository
            .Setup(r => r.DeleteAdminAsync(adminId))
            .Returns(Task.CompletedTask);

        await _adminService.DeleteAdmin(adminId);

        _mockAdminRepository.Verify(r => r.DeleteAdminAsync(adminId), Times.Once);
    }

    [Fact]
    public async Task DeleteAdmin_ShouldThrowException_WhenAdminDoesNotExist()
    {
        var adminId = Guid.NewGuid();

        _mockAdminRepository
            .Setup(r => r.GetAdminByIdAsync(adminId))
            .ReturnsAsync((Admin)null);

        await Assert.ThrowsAsync<AdminNotFoundException>(() => _adminService.DeleteAdmin(adminId));
    }

    public Task CreateAdmin(Admin adminException)
    {
        throw new NotImplementedException();
    }

    public Task<Admin> UpdateAdmin(Admin adminException)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAdmin(Guid id)
    {
        throw new NotImplementedException();
    }
}

