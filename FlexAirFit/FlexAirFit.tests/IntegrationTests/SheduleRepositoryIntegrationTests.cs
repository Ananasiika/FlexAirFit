using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;
using FlexAirFit.Database.Converters;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Models;
using IntegrationTests.DbFixtures;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Repositories.Tests
{
    public class ScheduleRepositoryTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleRepositoryTests()
        {
            _scheduleRepository = new ScheduleRepository(_dbContextFixture.Context);
        }

        [Fact]
        public async Task AddScheduleAsync_Should_Add_Schedule_To_Database()
        {
            // Arrange
            var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

            // Act
            await _scheduleRepository.AddScheduleAsync(schedule);

            // Assert
            var scheduleDbModel = await _dbContextFixture.Context.Schedules.FindAsync(schedule.Id);
            Assert.NotNull(scheduleDbModel);
            Assert.Equal(schedule.IdWorkout, scheduleDbModel.IdWorkout);
            Assert.Equal(schedule.DateAndTime, scheduleDbModel.DateAndTime);
            Assert.Equal(schedule.IdClient, scheduleDbModel.IdClient);
        }

        [Fact]
        public async Task UpdateScheduleAsync_Should_Update_Schedule_In_Database()
        {
            // Arrange
            var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());
            await _dbContextFixture.Context.Schedules.AddAsync(ScheduleConverter.CoreToDbModel(schedule));
            await _dbContextFixture.Context.SaveChangesAsync();

            var newIdWorkout = Guid.NewGuid();
            var newDateAndTime = DateTime.Now.AddDays(1);
            var newIdClient = Guid.NewGuid();

            // Act
            await _scheduleRepository.UpdateScheduleAsync(new Schedule(schedule.Id, newIdWorkout, newDateAndTime, newIdClient));

            // Assert
            var updatedScheduleDbModel = await _dbContextFixture.Context.Schedules.FindAsync(schedule.Id);
            Assert.NotNull(updatedScheduleDbModel);
            Assert.Equal(newIdWorkout, updatedScheduleDbModel.IdWorkout);
            Assert.Equal(newDateAndTime, updatedScheduleDbModel.DateAndTime);
            Assert.Equal(newIdClient, updatedScheduleDbModel.IdClient);
        }

        [Fact]
        public async Task DeleteScheduleAsync_Should_Delete_Schedule_From_Database()
        {
            // Arrange
            var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());
            await _dbContextFixture.Context.Schedules.AddAsync(ScheduleConverter.CoreToDbModel(schedule));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            await _scheduleRepository.DeleteScheduleAsync(schedule.Id);

            // Assert
            var scheduleDbModel = await _dbContextFixture.Context.Schedules.FindAsync(schedule.Id);
            Assert.Null(scheduleDbModel);
        }


        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
