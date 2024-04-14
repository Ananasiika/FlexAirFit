using FlexAirFit.Application.Services;
using FlexAirFit.Core.Models;
using FlexAirFit.Tests;
using IntegrationTests.DbFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IServices;
using FlexAirFit.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FlexAirFit.Application.Services.Tests
{
    public class BonusServiceTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IBonusService _bonusService;

        public BonusServiceTests()
        {
            _bonusService = new BonusService(new BonusRepository(_dbContextFixture.Context));
        }

        [Fact]
        public async Task CreateBonus_Should_Add_Bonus_To_Database()
        {
            // Arrange
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100);

            // Act
            await _bonusService.CreateBonus(bonus);

            // Assert
            var bonusDbModel = await _dbContextFixture.Context.Bonuses.FirstOrDefaultAsync(b => b.Id == bonus.Id);
            Assert.NotNull(bonusDbModel);
            Assert.Equal(bonus.IdClient, bonusDbModel.IdClient);
            Assert.Equal(bonus.Count, bonusDbModel.Count);
        }

        [Fact]
        public async Task UpdateBonus_Should_Update_Bonus_In_Database()
        {
            // Arrange
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100);
            await _bonusService.CreateBonus(bonus);

            bonus.Count = 200;

            // Act
            await _bonusService.UpdateBonus(bonus);

            // Assert
            var bonusDbModel = await _dbContextFixture.Context.Bonuses.FirstOrDefaultAsync(b => b.Id == bonus.Id);
            Assert.Equal(bonus.Count, bonusDbModel.Count);
        }

        [Fact]
        public async Task DeleteBonus_Should_Delete_Bonus_From_Database()
        {
            // Arrange
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100);
            await _bonusService.CreateBonus(bonus);

            // Act
            await _bonusService.DeleteBonus(bonus.Id);

            // Assert
            var bonusDbModel = await _dbContextFixture.Context.Bonuses.FirstOrDefaultAsync(b => b.Id == bonus.Id);
            Assert.Null(bonusDbModel);
        }

        [Fact]
        public async Task GetBonusById_Should_Return_Bonus_From_Database()
        {
            // Arrange
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100);
            await _bonusService.CreateBonus(bonus);

            // Act
            var result = await _bonusService.GetBonusById(bonus.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bonus.IdClient, result.IdClient);
            Assert.Equal(bonus.Count, result.Count);
        }

        [Fact]
        public async Task GetBonuses_Should_Return_List_Of_Bonuses_From_Database()
        {
            // Arrange
            var bonuses = new List<Bonus>
            {
                new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100),
                new Bonus(Guid.NewGuid(), Guid.NewGuid(), 200)
            };

            await _bonusService.CreateBonus(bonuses[0]);
            await _bonusService.CreateBonus(bonuses[1]);

            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _bonusService.GetBonuses(null, null);

            // Assert
            Assert.Equal(bonuses.Count, result.Count);
            for (int i = 0; i < bonuses.Count; i++)
            {
                Assert.Equal(bonuses[i].IdClient, result[i].IdClient);
                Assert.Equal(bonuses[i].Count, result[i].Count);
            }
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
