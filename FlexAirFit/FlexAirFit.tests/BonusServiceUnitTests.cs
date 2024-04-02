using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlexAirFit.Application.Services;
using Xunit;

namespace FlexAirFit.Tests
{
    public class BonusServiceUnitTests
    {
        private readonly Mock<IBonusRepository> _mockBonusRepository;
        private readonly IBonusService _bonusService;

        public BonusServiceUnitTests()
        {
            _mockBonusRepository = new Mock<IBonusRepository>();
            _bonusService = new BonusService(_mockBonusRepository.Object);
        }

        [Fact]
        public async Task CreateBonus_ShouldCallAddBonusAsync_WhenBonusDoesNotExist()
        {
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 10);

            _mockBonusRepository.Setup(r => r.GetBonusByIdAsync(bonus.Id)).ReturnsAsync((Bonus)null);

            await _bonusService.CreateBonus(bonus);

            _mockBonusRepository.Verify(r => r.AddBonusAsync(bonus), Times.Once);
        }

        [Fact]
        public async Task CreateBonus_ShouldThrowBonusExistsException_WhenBonusExists()
        {
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 10);

            _mockBonusRepository.Setup(r => r.GetBonusByIdAsync(bonus.Id)).ReturnsAsync(bonus);

            await Assert.ThrowsAsync<BonusExistsException>(() => _bonusService.CreateBonus(bonus));
        }

        [Fact]
        public async Task UpdateBonus_ShouldCallUpdateBonusAsync_WhenBonusExists()
        {
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 10);

            _mockBonusRepository.Setup(r => r.GetBonusByIdAsync(bonus.Id)).ReturnsAsync(bonus);

            await _bonusService.UpdateBonus(bonus);

            _mockBonusRepository.Verify(r => r.UpdateBonusAsync(bonus), Times.Once);
        }

        [Fact]
        public async Task UpdateBonus_ShouldThrowBonusNotFoundException_WhenBonusDoesNotExist()
        {
            var bonus = new Bonus(Guid.NewGuid(), Guid.NewGuid(), 10);

            _mockBonusRepository.Setup(r => r.GetBonusByIdAsync(bonus.Id)).ReturnsAsync((Bonus)null);

            await Assert.ThrowsAsync<BonusNotFoundException>(() => _bonusService.UpdateBonus(bonus));
        }

        [Fact]
        public async Task DeleteBonus_ShouldCallDeleteBonusAsync_WhenBonusExists()
        {
            var bonusId = Guid.NewGuid();

            _mockBonusRepository.Setup(r => r.GetBonusByIdAsync(bonusId)).ReturnsAsync(new Bonus(bonusId, Guid.NewGuid(), 10));

            await _bonusService.DeleteBonus(bonusId);

            _mockBonusRepository.Verify(r => r.DeleteBonusAsync(bonusId), Times.Once);
        }

        [Fact]
        public async Task DeleteBonus_ShouldThrowBonusNotFoundException_WhenBonusDoesNotExist()
        {
            var bonusId = Guid.NewGuid();

            _mockBonusRepository.Setup(r => r.GetBonusByIdAsync(bonusId)).ReturnsAsync((Bonus)null);

            await Assert.ThrowsAsync<BonusNotFoundException>(() => _bonusService.DeleteBonus(bonusId));
        }

        [Fact]
        public async Task GetBonusById_ShouldReturnBonus_WhenBonusExists()
        {
            var bonusId = Guid.NewGuid();
            var bonus = new Bonus(bonusId, Guid.NewGuid(), 10);

            _mockBonusRepository.Setup(r => r.GetBonusByIdAsync(bonusId)).ReturnsAsync(bonus);

            var result = await _bonusService.GetBonusById(bonusId);

            Assert.Equal(bonus, result);
        }

        [Fact]

        public async Task GetBonusById_ShouldThrowBonusNotFoundException_WhenBonusDoesNotExist()
        {
            var bonusId = Guid.NewGuid();

            _mockBonusRepository.Setup(r => r.GetBonusByIdAsync(bonusId)).ReturnsAsync((Bonus)null);

            await Assert.ThrowsAsync<BonusNotFoundException>(() => _bonusService.GetBonusById(bonusId));
        }

        [Fact]
        public async Task GetBonuses_ShouldReturnListOfBonuses_WithLimitAndOffset()
        {
            var bonuses = new List<Bonus>
            {
                new Bonus(Guid.NewGuid(), Guid.NewGuid(), 10),
                new Bonus(Guid.NewGuid(), Guid.NewGuid(), 20),
                new Bonus(Guid.NewGuid(), Guid.NewGuid(), 30)
            };

            _mockBonusRepository.Setup(r => r.GetBonusesAsync(2, 1)).ReturnsAsync(bonuses);

            var result = await _bonusService.GetBonuses(2, 1);

            Assert.Equal(bonuses, result);
        }

        [Fact]
        public async Task GetCountBonusByIdClient_ShouldReturnCountOfBonuses()
        {
            var clientId = Guid.NewGuid();
            var count = 5;

            _mockBonusRepository.Setup(r => r.GetCountBonusByIdClientAsync(clientId)).ReturnsAsync(count);

            var result = await _bonusService.GetCountBonusByIdClient(clientId);

            Assert.Equal(count, result);
        }
    }
}
