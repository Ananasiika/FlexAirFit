using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Filters;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlexAirFit.Application.Services;
using Xunit;

namespace FlexAirFit.Tests
{
    public class SheduleServiceUnitTests
    {
        private readonly Mock<ISheduleRepository> _mockSheduleRepository;
        private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
        private readonly ISheduleService _sheduleService;

        public SheduleServiceUnitTests()
        {
            _mockSheduleRepository = new Mock<ISheduleRepository>();
            _mockWorkoutRepository = new Mock<IWorkoutRepository>();
            _sheduleService = new SheduleService(_mockSheduleRepository.Object, _mockWorkoutRepository.Object);
        }

        [Fact]
        public async Task CreateShedule_ShouldCallAddSheduleAsync_WhenSheduleDoesNotExist()
        {
            var shedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

            _mockSheduleRepository.Setup(r => r.GetSheduleByIdAsync(shedule.Id)).ReturnsAsync((Schedule)null);

            await _sheduleService.CreateShedule(shedule);

            _mockSheduleRepository.Verify(r => r.AddSheduleAsync(shedule), Times.Once);
        }

        [Fact]
        public async Task CreateShedule_ShouldThrowSheduleExistsException_WhenSheduleExists()
        {
            var shedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

            _mockSheduleRepository.Setup(r => r.GetSheduleByIdAsync(shedule.Id)).ReturnsAsync(shedule);

            await Assert.ThrowsAsync<SheduleExistsException>(() => _sheduleService.CreateShedule(shedule));
        }

        [Fact]
        public async Task UpdateShedule_ShouldCallUpdateSheduleAsync_WhenSheduleExists()
        {
            var shedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

            _mockSheduleRepository.Setup(r => r.GetSheduleByIdAsync(shedule.Id)).ReturnsAsync(shedule);

            await _sheduleService.UpdateShedule(shedule);

            _mockSheduleRepository.Verify(r => r.UpdateSheduleAsync(shedule), Times.Once);
        }

        [Fact]
        public async Task UpdateShedule_ShouldThrowSheduleNotFoundException_WhenSheduleDoesNotExist()
        {
            var shedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

            _mockSheduleRepository.Setup(r => r.GetSheduleByIdAsync(shedule.Id)).ReturnsAsync((Schedule)null);

            await Assert.ThrowsAsync<SheduleNotFoundException>(() => _sheduleService.UpdateShedule(shedule));
        }

        [Fact]
        public async Task DeleteShedule_ShouldCallDeleteSheduleAsync_WhenSheduleExists()
        {
            var sheduleId = Guid.NewGuid();

            _mockSheduleRepository.Setup(r => r.GetSheduleByIdAsync(sheduleId)).ReturnsAsync(new Schedule(sheduleId, Guid.NewGuid(), DateTime.Now, Guid.NewGuid()));

            await _sheduleService.DeleteShedule(sheduleId);

            _mockSheduleRepository.Verify(r => r.DeleteSheduleAsync(sheduleId), Times.Once);
        }

        [Fact]
        public async Task DeleteShedule_ShouldThrowSheduleNotFoundException_WhenSheduleDoesNotExist()
        {
            var sheduleId = Guid.NewGuid();

            _mockSheduleRepository.Setup(r => r.GetSheduleByIdAsync(sheduleId)).ReturnsAsync((Schedule)null);

            await Assert.ThrowsAsync<SheduleNotFoundException>(() => _sheduleService.DeleteShedule(sheduleId));
        }

        [Fact]
        public async Task GetSheduleById_ShouldReturnShedule_WhenSheduleExists()
        {
            var sheduleId = Guid.NewGuid();
            var shedule = new Schedule(sheduleId, Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

            _mockSheduleRepository.Setup(r => r.GetSheduleByIdAsync(sheduleId)).ReturnsAsync(shedule);

            var result = await _sheduleService.GetSheduleById(sheduleId);

            Assert.Equal(shedule, result);
        }

        [Fact]
        public async Task GetSheduleById_ShouldThrowSheduleNotFoundException_WhenSheduleDoesNotExist()
        {
            var sheduleId = Guid.NewGuid();

            _mockSheduleRepository.Setup(r => r.GetSheduleByIdAsync(sheduleId)).ReturnsAsync((Schedule)null);

            await Assert.ThrowsAsync<SheduleNotFoundException>(() => _sheduleService.GetSheduleById(sheduleId));
        }

        [Fact]
        public async Task GetShedules_ShouldReturnListOfShedules_WithLimitAndOffset()
        {
            var shedules = new List<Schedule>
            {
                new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid()),
                new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid()),
                new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid())
            };

            _mockSheduleRepository.Setup(r => r.GetShedulesAsync(2, 1)).ReturnsAsync(shedules);

            var result = await _sheduleService.GetShedules(2, 1);

            Assert.Equal(shedules, result);
        }
    }
}
