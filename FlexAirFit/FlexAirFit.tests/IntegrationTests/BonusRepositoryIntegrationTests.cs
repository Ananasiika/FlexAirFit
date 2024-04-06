using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Converters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FlexAirFit.Tests;
using IntegrationTests.DbFixtures;

namespace FlexAirFit.Database.Repositories.Tests
{
    public class BonusRepositoryTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IBonusRepository _bonusRepository;

        public BonusRepositoryTests()
        {
            _bonusRepository = new BonusRepository(_dbContextFixture.Context);
        }

        [Fact]
        public async Task AddBonusAsync_Should_Add_Bonus_To_Database()
        {
            // Arrange
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100);

            // Act
            await _bonusRepository.AddBonusAsync(bonus);

            // Assert
            var bonusDbModel = await _dbContextFixture.Context.Bonuses.FirstOrDefaultAsync(b => b.Id == bonus.Id);
            Assert.NotNull(bonusDbModel);
            Assert.Equal(bonus.IdClient, bonusDbModel.IdClient);
            Assert.Equal(bonus.Count, bonusDbModel.Count);
        }

        [Fact]
        public async Task UpdateBonusAsync_Should_Update_Bonus_In_Database()
        {
            // Arrange
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100);
            await _dbContextFixture.Context.Bonuses.AddAsync(BonusConverter.CoreToDbModel(bonus));
            await _dbContextFixture.Context.SaveChangesAsync();

            bonus.Count = 200;

            // Act
            await _bonusRepository.UpdateBonusAsync(bonus);

            // Assert
            var bonusDbModel = await _dbContextFixture.Context.Bonuses.FirstOrDefaultAsync(b => b.Id == bonus.Id);
            Assert.Equal(bonus.Count, bonusDbModel.Count);
        }

        [Fact]
        public async Task DeleteBonusAsync_Should_Delete_Bonus_From_Database()
        {
            // Arrange
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100);
            await _dbContextFixture.Context.Bonuses.AddAsync(BonusConverter.CoreToDbModel(bonus));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            await _bonusRepository.DeleteBonusAsync(bonus.Id);

            // Assert
            var bonusDbModel = await _dbContextFixture.Context.Bonuses.FirstOrDefaultAsync(b => b.Id == bonus.Id);
            Assert.Null(bonusDbModel);
        }

        [Fact]
        public async Task GetBonusByIdAsync_Should_Return_Bonus_From_Database()
        {
            // Arrange
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100);
            await _dbContextFixture.Context.Bonuses.AddAsync(BonusConverter.CoreToDbModel(bonus));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _bonusRepository.GetBonusByIdAsync(bonus.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bonus.IdClient, result.IdClient);
            Assert.Equal(bonus.Count, result.Count);
        }

        [Fact]
        public async Task GetBonusesAsync_Should_Return_List_Of_Bonuses_From_Database()
        {
            // Arrange
            var bonuses = new List<Bonus>
            {
                new Bonus(Guid.NewGuid(), Guid.NewGuid(), 100),
                new Bonus(Guid.NewGuid(), Guid.NewGuid(), 200)
            };

            await _dbContextFixture.Context.Bonuses.AddRangeAsync(bonuses.Select(BonusConverter.CoreToDbModel));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _bonusRepository.GetBonusesAsync(null, null);

            // Assert
            Assert.Equal(bonuses.Count, result.Count);
            for (int i = 0; i < bonuses.Count; i++)
            {
                Assert.Equal(bonuses[i].IdClient, result[i].IdClient);
                Assert.Equal(bonuses[i].Count, result[i].Count);
            }
        }

        [Fact]
        public async Task GetCountBonusByIdClientAsync_Should_Return_Count_Of_Bonuses_By_Client_Id_From_Database()
        {
            // Arrange
            var clientid = Guid.NewGuid();
            var bonuses = new List<Bonus>
            {
                new Bonus(Guid.NewGuid(), clientid, 100),
                new Bonus(Guid.NewGuid(), clientid, 200)
            };

            await _dbContextFixture.Context.Bonuses.AddRangeAsync(bonuses.Select(BonusConverter.CoreToDbModel));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _bonusRepository.GetCountBonusByIdClientAsync(clientid);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task UpdateCountBonusByIdClientAsync_Should_Update_Bonus_Count_By_Client_Id_In_Database()
        {
            // Arrange
            var clientid = Guid.NewGuid();
            var bonus = new Bonus(Guid.NewGuid(), clientid, 100);
            await _dbContextFixture.Context.Bonuses.AddAsync(BonusConverter.CoreToDbModel(bonus));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            await _bonusRepository.UpdateCountBonusByIdClientAsync(clientid, 200);

            // Assert
            var bonusDbModel = await _dbContextFixture.Context.Bonuses.FirstOrDefaultAsync(b => b.IdClient == clientid);
            Assert.Equal(200, bonusDbModel.Count);
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
