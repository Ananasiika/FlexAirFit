using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;
using FlexAirFit.Database.Repositories;
using IntegrationTests.DbFixtures;

namespace FlexAirFit.Application.Services.Tests
{
    public class ScheduleServiceTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IScheduleService _scheduleService;

        public ScheduleServiceTests()
        {
            _scheduleService = new ScheduleService(new ScheduleRepository(_dbContextFixture.Context),
                                                   new WorkoutRepository(_dbContextFixture.Context));
        }

        [Fact]
        public async Task CreateSchedule_Should_Add_Schedule_To_Database()
        {
            // Arrange
            var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

            // Act
            await _scheduleService.CreateSchedule(schedule);

            // Assert
            var scheduleDbModel = await _dbContextFixture.Context.Schedules.FindAsync(schedule.Id);
            Assert.NotNull(scheduleDbModel);
            Assert.Equal(schedule.IdWorkout, scheduleDbModel.IdWorkout);
            Assert.Equal(schedule.DateAndTime, scheduleDbModel.DateAndTime);
            Assert.Equal(schedule.IdClient, scheduleDbModel.IdClient);
        }

        [Fact]
        public async Task UpdateSchedule_Should_Update_Schedule_In_Database()
        {
            // Arrange
            var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());
            await _scheduleService.CreateSchedule(schedule);
            
            schedule.IdWorkout = Guid.NewGuid();
            schedule.DateAndTime = DateTime.Now.AddDays(1);
            schedule.IdClient = Guid.NewGuid();

            // Act
            await _scheduleService.UpdateSchedule(schedule);

            // Assert
            var updatedScheduleDbModel = await _dbContextFixture.Context.Schedules.FindAsync(schedule.Id);
            Assert.NotNull(updatedScheduleDbModel);
            Assert.Equal(schedule.IdWorkout, updatedScheduleDbModel.IdWorkout);
            Assert.Equal(schedule.DateAndTime, updatedScheduleDbModel.DateAndTime);
            Assert.Equal(schedule.IdClient, updatedScheduleDbModel.IdClient);
        }

        [Fact]
        public async Task DeleteSchedule_Should_Delete_Schedule_From_Database()
        {
            // Arrange
            var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());
            await _scheduleService.CreateSchedule(schedule);

            // Act
            await _scheduleService.DeleteSchedule(schedule.Id);

            // Assert
            var scheduleDbModel = await _dbContextFixture.Context.Schedules.FindAsync(schedule.Id);
            Assert.Null(scheduleDbModel);
        }

        [Fact]
        public async Task GetScheduleById_Should_Return_Schedule_From_Database()
        {
            // Arrange
            var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());
            await _scheduleService.CreateSchedule(schedule);

            // Act

            var result = await _scheduleService.GetScheduleById(schedule.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(schedule.IdWorkout, result.IdWorkout);
            Assert.Equal(schedule.DateAndTime, result.DateAndTime);
            Assert.Equal(schedule.IdClient, result.IdClient);
        }

        [Fact]
        public async Task GetSchedules_Should_Return_List_Of_Schedules_From_Database()
        {
            // Arrange
            var schedules = new List<Schedule>
            {
                new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid()),
                new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1), Guid.NewGuid())
            };

            await _scheduleService.CreateSchedule(schedules[0]);
            await _scheduleService.CreateSchedule(schedules[1]);

            // Act
            var result = await _scheduleService.GetSchedules(null, null);

            // Assert
            Assert.Equal(schedules.Count, result.Count);
            for (int i = 0; i < schedules.Count; i++)
            {
                Assert.Equal(schedules[i].IdWorkout, result[i].IdWorkout);
                Assert.Equal(schedules[i].DateAndTime, result[i].DateAndTime);
                Assert.Equal(schedules[i].IdClient, result[i].IdClient);
            }
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
