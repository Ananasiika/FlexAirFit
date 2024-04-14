using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Repositories;
using IntegrationTests.DbFixtures;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Application.Services.Tests
{
    public class MembershipServiceTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IMembershipService _membershipService;

        public MembershipServiceTests()
        {
            _membershipService = new MembershipService(new MembershipRepository(_dbContextFixture.Context));
        }

        [Fact]
        public async Task CreateMembership_Should_Add_Membership_To_Database()
        {
            // Arrange
            var membership = new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0);

            // Act
            await _membershipService.CreateMembership(membership);

            // Assert
            var membershipDbModel = await _dbContextFixture.Context.Memberships.FirstOrDefaultAsync(m => m.Id == membership.Id);
            Assert.NotNull(membershipDbModel);
            Assert.Equal(membership.Name, membershipDbModel.Name);
            Assert.Equal(membership.Duration, membershipDbModel.Duration);
            Assert.Equal(membership.Price, membershipDbModel.Price);
            Assert.Equal(membership.Freezing, membershipDbModel.Freezing);
        }

        [Fact]
        public async Task UpdateMembership_Should_Update_Membership_In_Database()
        {
            // Arrange
            var membership = new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0);
            await _membershipService.CreateMembership(membership);

            membership.Name = "Platinum";
            membership.Duration = TimeSpan.FromDays(60);
            membership.Price = 300;
            membership.Freezing = 7;

            // Act
            await _membershipService.UpdateMembership(membership);

            // Assert
            var membershipDbModel = await _dbContextFixture.Context.Memberships.FirstOrDefaultAsync(m => m.Id == membership.Id);
            Assert.Equal(membership.Name, membershipDbModel.Name);
            Assert.Equal(membership.Duration, membershipDbModel.Duration);
            Assert.Equal(membership.Price, membershipDbModel.Price);
            Assert.Equal(membership.Freezing, membershipDbModel.Freezing);
        }

        [Fact]
        public async Task DeleteMembership_Should_Delete_Membership_From_Database()
        {
            // Arrange
            var membership = new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0);
            await _membershipService.CreateMembership(membership);

            // Act
            await _membershipService.DeleteMembership(membership.Id);

            // Assert
            var membershipDbModel = await _dbContextFixture.Context.Memberships.FirstOrDefaultAsync(m => m.Id == membership.Id);
            Assert.Null(membershipDbModel);
        }

        [Fact]
        public async Task GetMembershipById_Should_Return_Membership_From_Database()
        {
            // Arrange
            var membership = new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0);

            await _membershipService.CreateMembership(membership);

            // Act
            var result = await _membershipService.GetMembershipById(membership.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(membership.Name, result.Name);
            Assert.Equal(membership.Duration, result.Duration);
            Assert.Equal(membership.Price, result.Price);
            Assert.Equal(membership.Freezing, result.Freezing);
        }

        [Fact]
        public async Task GetMemberships_Should_Return_List_Of_Memberships_From_Database()
        {
            // Arrange
            var memberships = new List<Membership>
            {
                new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0),
                new Membership(Guid.NewGuid(), "Platinum", TimeSpan.FromDays(60), 300, 7)
            };

            await _membershipService.CreateMembership(memberships[0]);
            await _membershipService.CreateMembership(memberships[1]);

            // Act
            var result = await _membershipService.GetMemberships(null, null);

            // Assert
            Assert.Equal(memberships.Count, result.Count);
            for (int i = 0; i < memberships.Count; i++)
            {
                Assert.Equal(memberships[i].Name, result[i].Name);
                Assert.Equal(memberships[i].Duration, result[i].Duration);
                Assert.Equal(memberships[i].Price, result[i].Price);
                Assert.Equal(memberships[i].Freezing, result[i].Freezing);
            }
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
