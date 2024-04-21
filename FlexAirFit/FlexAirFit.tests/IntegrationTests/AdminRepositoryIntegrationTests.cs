using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Services;
using FlexAirFit.Database.Repositories;
using IntegrationTests.DbFixtures;
using Microsoft.EntityFrameworkCore;

public class AdminServiceTests : IDisposable
{
    private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
    private readonly IAdminService _adminService;

    public AdminServiceTests()
    {
        _adminService = new AdminService(new AdminRepository(_dbContextFixture.Context));
    }

    [Fact]
    public async Task CreateAdmin_Should_Add_Admin_To_Database()
    {
        // Arrange
        var admin = new Admin(Guid.NewGuid(), "John Doe", new DateTime(1990, 1, 1), "Male");

        // Act
        await _adminService.CreateAdmin(admin);

        // Assert
        var adminDbModel = await _dbContextFixture.Context.Admins.FirstOrDefaultAsync(a => a.Id == admin.Id);
        Assert.NotNull(adminDbModel);
        Assert.Equal(admin.Name, adminDbModel.Name);
        Assert.Equal(admin.DateOfBirth, adminDbModel.DateOfBirth);
        Assert.Equal(admin.Gender, adminDbModel.Gender);
    }

    [Fact]
    public async Task UpdateAdmin_Should_Update_Admin_In_Database()
    {
        // Arrange
        var admin = new Admin(Guid.NewGuid(), "John Doe", new DateTime(1990, 1, 1), "Male");
        await _adminService.CreateAdmin(admin);

        admin.Name = "Updated Name";

        // Act
        var updatedAdmin = await _adminService.UpdateAdmin(admin);

        // Assert
        var adminDbModel = await _dbContextFixture.Context.Admins.FirstOrDefaultAsync(a => a.Id == admin.Id);
        Assert.NotNull(adminDbModel);
        Assert.Equal(admin.Name, adminDbModel.Name);
    }


    [Fact]
    public async Task DeleteAdmin_Should_Delete_Admin_From_Database()
    {
        // Arrange
        var admin = new Admin(Guid.NewGuid(), "John Doe", new DateTime(1990, 1, 1), "Male");
        await _adminService.CreateAdmin(admin);

        // Act
        await _adminService.DeleteAdmin(admin.Id);

        // Assert
        var adminDbModel = await _dbContextFixture.Context.Admins.FirstOrDefaultAsync(a => a.Id == admin.Id);
        Assert.Null(adminDbModel);
    }

    public void Dispose()
    {
        _dbContextFixture.Dispose();
    }
}
