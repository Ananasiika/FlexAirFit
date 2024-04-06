using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Filters;
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
    public class WorkoutRepositoryTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutRepositoryTests()
        {
            _workoutRepository = new WorkoutRepository(_dbContextFixture.Context);
        }

        [Fact]
        public async Task AddWorkoutAsync_Should_Add_Workout_To_Database()
        {
            // Arrange
            var workout = new Workout(Guid.NewGuid(), "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);

            // Act
            await _workoutRepository.AddWorkoutAsync(workout);

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
            await _dbContextFixture.Context.Workouts.AddAsync(WorkoutConverter.CoreToDbModel(workout));
            await _dbContextFixture.Context.SaveChangesAsync();

            workout.Name = "Updated Workout";
            workout.Description = "Updated Description";
            workout.IdTrainer = Guid.NewGuid();
            workout.Duration = TimeSpan.FromMinutes(90);
            workout.Level = 7;

            // Act
            await _workoutRepository.UpdateWorkoutAsync(workout);

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
            await _dbContextFixture.Context.Workouts.AddAsync(WorkoutConverter.CoreToDbModel(workout));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            await _workoutRepository.DeleteWorkoutAsync(workout.Id);

            // Assert
            var workoutDbModel = await _dbContextFixture.Context.Workouts.FindAsync(workout.Id);
            Assert.Null(workoutDbModel);
        }

        [Fact]

        public async Task GetWorkoutByFilterAsync_Should_Return_Filtered_Workouts_From_Database()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout(Guid.NewGuid(), "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5),
                new Workout(Guid.NewGuid(), "Workout 2", "Description 2", Guid.NewGuid(), TimeSpan.FromMinutes(45), 3),
                new Workout(Guid.NewGuid(), "Workout 3", "Description 3", Guid.NewGuid(), TimeSpan.FromMinutes(90), 8),
                new Workout(Guid.NewGuid(), "Workout 4", "Description 4", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5),
                new Workout(Guid.NewGuid(), "Workout 5", "Description 5", Guid.NewGuid(), TimeSpan.FromMinutes(75), 5),
            };

            await _dbContextFixture.Context.Workouts.AddRangeAsync(workouts.Select(WorkoutConverter.CoreToDbModel));
            await _dbContextFixture.Context.SaveChangesAsync();

            var filter = new FilterWorkout(null, null, TimeSpan.FromMinutes(50), TimeSpan.FromMinutes(90), 4, 6);

            // Act
            var result = await _workoutRepository.GetWorkoutByFilterAsync(filter);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, w => w.Name == "Workout 1");
            Assert.Contains(result, w => w.Name == "Workout 5");
        }

        [Fact]
        public async Task GetWorkoutByIdAsync_Should_Return_Workout_From_Database()
        {
            // Arrange
            var workout = new Workout(Guid.NewGuid(), "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);
            await _dbContextFixture.Context.Workouts.AddAsync(WorkoutConverter.CoreToDbModel(workout));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _workoutRepository.GetWorkoutByIdAsync(workout.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(workout.Name, result.Name);
            Assert.Equal(workout.Description, result.Description);
            Assert.Equal(workout.IdTrainer, result.IdTrainer);
            Assert.Equal(workout.Duration, result.Duration);
            Assert.Equal(workout.Level, result.Level);
        }

        [Fact]
        public async Task GetWorkoutsAsync_Should_Return_List_Of_Workouts_From_Database()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout(Guid.NewGuid(), "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5),
                new Workout(Guid.NewGuid(), "Workout 2", "Description 2", Guid.NewGuid(), TimeSpan.FromMinutes(45), 3),
                new Workout(Guid.NewGuid(), "Workout 3", "Description 3", Guid.NewGuid(), TimeSpan.FromMinutes(90), 8),
            };

            await _dbContextFixture.Context.Workouts.AddRangeAsync(workouts.Select(WorkoutConverter.CoreToDbModel));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _workoutRepository.GetWorkoutsAsync(null, null);

            // Assert
            Assert.Equal(workouts.Count, result.Count);
            for (int i = 0; i < workouts.Count; i++)
            {
                Assert.Equal(workouts[i].Name, result[i].Name);
                Assert.Equal(workouts[i].Description, result[i].Description);
                Assert.Equal(workouts[i].IdTrainer, result[i].IdTrainer);

                Assert.Equal(workouts[i].Duration, result[i].Duration);
                Assert.Equal(workouts[i].Level, result[i].Level);
            }
        }

        [Fact]
        public async Task GetWorkoutNameByIdAsync_Should_Return_Workout_Name_From_Database()
        {
            // Arrange
            var workout = new Workout(Guid.NewGuid(), "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);
            await _dbContextFixture.Context.Workouts.AddAsync(WorkoutConverter.CoreToDbModel(workout));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _workoutRepository.GetWorkoutNameByIdAsync(workout.Id);

            // Assert
            Assert.Equal(workout.Name, result);
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
