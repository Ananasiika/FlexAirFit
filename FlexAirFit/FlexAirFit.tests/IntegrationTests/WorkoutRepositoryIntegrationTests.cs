using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Services;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Repositories;
using IntegrationTests.DbFixtures;
using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Services.Tests
{
    public class WorkoutServiceTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IWorkoutService _workoutService;

        public WorkoutServiceTests()
        {
            _workoutService = new WorkoutService(new WorkoutRepository(_dbContextFixture.Context),
                                                 new TrainerRepository(_dbContextFixture.Context));
        }

        [Fact]
        public async Task AddWorkoutAsync_Should_Add_Workout_To_Database()
        {
            // Arrange
            var workout = new Workout(Guid.NewGuid(), "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);

            // Act
            await _workoutService.CreateWorkout(workout);

            // Assert
            var workoutDbModel = await _dbContextFixture.Context.Workouts.FindAsync(workout.Id);
            Assert.NotNull(workoutDbModel);
            Assert.Equal(workout.Name, workoutDbModel.Name);
            Assert.Equal(workout.Description, workoutDbModel.Description);
            Assert.Equal(workout.IdTrainer, workoutDbModel.IdTrainer);
            Assert.Equal(workout.Duration, workoutDbModel.Duration);
            Assert.Equal(workout.Level, workoutDbModel.Level);
        }

        [Fact]
        public async Task UpdateWorkoutAsync_Should_Update_Workout_In_Database()
        {
            // Arrange
            var workout = new Workout(Guid.NewGuid(), "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);
            await _workoutService.CreateWorkout(workout);

            workout.Name = "Updated Workout";
            workout.Description = "Updated Description";
            workout.IdTrainer = Guid.NewGuid();
            workout.Duration = TimeSpan.FromMinutes(90);
            workout.Level = 7;

            // Act
            await _workoutService.UpdateWorkout(workout);

            // Assert
            var workoutDbModel = await _dbContextFixture.Context.Workouts.FindAsync(workout.Id);
            Assert.Equal(workout.Name, workoutDbModel.Name);
            Assert.Equal(workout.Description, workoutDbModel.Description);
            Assert.Equal(workout.IdTrainer, workoutDbModel.IdTrainer);
            Assert.Equal(workout.Duration, workoutDbModel.Duration);
            Assert.Equal(workout.Level, workoutDbModel.Level);
        }

        [Fact]
        public async Task DeleteWorkoutAsync_Should_Delete_Workout_From_Database()
        {
            // Arrange
            var workout = new Workout(Guid.NewGuid(), "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);
            await _workoutService.CreateWorkout(workout);

            // Act
            await _workoutService.DeleteWorkout(workout.Id);
            
            // Assert
            var deletedWorkout = await _dbContextFixture.Context.Workouts.FindAsync(workout.Id);
            Assert.Null(deletedWorkout);
        }

        public void Dispose()
        {
            _dbContextFixture?.Dispose();
        }
    }
}
