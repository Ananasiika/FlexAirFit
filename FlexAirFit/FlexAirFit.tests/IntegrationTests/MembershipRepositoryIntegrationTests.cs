using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Converters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.DbFixtures;
using Xunit;

namespace FlexAirFit.Database.Repositories.Tests
{
    public class MembershipRepositoryTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IMembershipRepository _membershipRepository;

        public MembershipRepositoryTests()
        {
            _membershipRepository = new MembershipRepository(_dbContextFixture.Context);
        }

        [Fact]
        public async Task AddMembershipAsync_Should_Add_Membership_To_Database()
        {
            // Arrange
            var membership = new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0);

            // Act
            await _membershipRepository.AddMembershipAsync(membership);

            // Assert
            var membershipDbModel = await _dbContextFixture.Context.Memberships.FirstOrDefaultAsync(m => m.Id == membership.Id);
            Assert.NotNull(membershipDbModel);
            Assert.Equal(membership.Name, membershipDbModel.Name);
            Assert.Equal(membership.Duration, membershipDbModel.Duration);
            Assert.Equal(membership.Price, membershipDbModel.Price);
            Assert.Equal(membership.Freezing, membershipDbModel.Freezing);
        }

        [Fact]
        public async Task UpdateMembershipAsync_Should_Update_Membership_In_Database()
        {
            // Arrange
            var membership = new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0);
            await _dbContextFixture.Context.Memberships.AddAsync(MembershipConverter.CoreToDbModel(membership));
            await _dbContextFixture.Context.SaveChangesAsync();

            membership.Name = "Platinum";
            membership.Duration = TimeSpan.FromDays(60);
            membership.Price = 300;
            membership.Freezing = 7;

            // Act
            await _membershipRepository.UpdateMembershipAsync(membership);

            // Assert
            var membershipDbModel = await _dbContextFixture.Context.Memberships.FirstOrDefaultAsync(m => m.Id == membership.Id);
            Assert.Equal(membership.Name, membershipDbModel.Name);
            Assert.Equal(membership.Duration, membershipDbModel.Duration);
            Assert.Equal(membership.Price, membershipDbModel.Price);
            Assert.Equal(membership.Freezing, membershipDbModel.Freezing);
        }

        [Fact]
        public async Task DeleteMembershipAsync_Should_Delete_Membership_From_Database()
        {
            // Arrange
            var membership = new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0);
            await _dbContextFixture.Context.Memberships.AddAsync(MembershipConverter.CoreToDbModel(membership));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            await _membershipRepository.DeleteMembershipAsync(membership.Id);

            // Assert
            var membershipDbModel = await _dbContextFixture.Context.Memberships.FirstOrDefaultAsync(m => m.Id == membership.Id);
            Assert.Null(membershipDbModel);
        }

        [Fact]
        public async Task GetMembershipByIdAsync_Should_Return_Membership_From_Database()
        {
            // Arrange

            var membership = new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0);
            await _dbContextFixture.Context.Memberships.AddAsync(MembershipConverter.CoreToDbModel(membership));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _membershipRepository.GetMembershipByIdAsync(membership.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(membership.Name, result.Name);
            Assert.Equal(membership.Duration, result.Duration);
            Assert.Equal(membership.Price, result.Price);
            Assert.Equal(membership.Freezing, result.Freezing);
        }

        [Fact]
        public async Task GetMembershipsAsync_Should_Return_List_Of_Memberships_From_Database()
        {
            // Arrange
            var memberships = new List<Membership>
            {
                new Membership(Guid.NewGuid(), "Gold", TimeSpan.FromDays(30), 200, 0),
                new Membership(Guid.NewGuid(), "Silver", TimeSpan.FromDays(60), 150, 7)
            };

            await _dbContextFixture.Context.Memberships.AddRangeAsync(memberships.Select(MembershipConverter.CoreToDbModel));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _membershipRepository.GetMembershipsAsync(null, null);

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
