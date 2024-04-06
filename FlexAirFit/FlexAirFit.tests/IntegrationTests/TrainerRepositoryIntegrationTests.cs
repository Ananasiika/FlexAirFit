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
    public class TrainerRepositoryTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly ITrainerRepository _trainerRepository;

        public TrainerRepositoryTests()
        {
            _trainerRepository = new TrainerRepository(_dbContextFixture.Context);
        }

        [Fact]
        public async Task AddTrainerAsync_Should_Add_Trainer_To_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var specialization = "Fitness";
            var experience = 5;
            var rating = 4;

            var trainer = new Trainer(id, userId, name, gender, specialization, experience, rating);

            // Act
            await _trainerRepository.AddTrainerAsync(trainer);

            // Assert
            var trainerDbModel = await _dbContextFixture.Context.Trainers.FindAsync(trainer.Id);
            Assert.NotNull(trainerDbModel);
            Assert.Equal(trainer.IdUser, trainerDbModel.IdUser);
            Assert.Equal(trainer.Name, trainerDbModel.Name);
            Assert.Equal(trainer.Gender, trainerDbModel.Gender);
            Assert.Equal(trainer.Specialization, trainerDbModel.Specialization);
            Assert.Equal(trainer.Experience, trainerDbModel.Experience);
            Assert.Equal(trainer.Rating, trainerDbModel.Rating);
        }

        [Fact]
        public async Task UpdateTrainerAsync_Should_Update_Trainer_In_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var specialization = "Fitness";
            var experience = 5;
            var rating = 4;

            var trainer = new Trainer(id, userId, name, gender, specialization, experience, rating);

            await _dbContextFixture.Context.Trainers.AddAsync(TrainerConverter.CoreToDbModel(trainer));
            await _dbContextFixture.Context.SaveChangesAsync();

            var newName = "Jane Smith";
            var newRating = 5;

            // Act
            var updatedTrainer = await _trainerRepository.UpdateTrainerAsync(new Trainer(id, userId, newName, gender, specialization, experience, newRating));

            // Assert
            var updatedTrainerDbModel = await _dbContextFixture.Context.Trainers.FindAsync(id);
            Assert.NotNull(updatedTrainerDbModel);
            Assert.Equal(newName, updatedTrainerDbModel.Name);
            Assert.Equal(newRating, updatedTrainerDbModel.Rating);
            Assert.Equal(newName, updatedTrainer.Name);
            Assert.Equal(newRating, updatedTrainer.Rating);
        }

        [Fact]
        public async Task DeleteTrainerAsync_Should_Delete_Trainer_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var specialization = "Fitness";
            var experience = 5;

            var rating = 4;

            var trainer = new Trainer(id, userId, name, gender, specialization, experience, rating);

            await _dbContextFixture.Context.Trainers.AddAsync(TrainerConverter.CoreToDbModel(trainer));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            await _trainerRepository.DeleteTrainerAsync(trainer.Id);

            // Assert
            var trainerDbModel = await _dbContextFixture.Context.Trainers.FindAsync(trainer.Id);
            Assert.Null(trainerDbModel);
        }

        [Fact]
        public async Task GetTrainerByFilterAsync_Should_Return_Filtered_Trainers_From_Database()
        {
            // Arrange 
            var trainers = new List<Trainer>
            {
                new Trainer(Guid.NewGuid(), Guid.NewGuid(), "John Doe", "Male", "Fitness", 5, 4),
                new Trainer(Guid.NewGuid(), Guid.NewGuid(), "Jane Smith", "Female", "Yoga", 3, 5),
                new Trainer(Guid.NewGuid(), Guid.NewGuid(), "Mark Johnson", "Male", "Crossfit", 7, 4),
            };

            await _dbContextFixture.Context.Trainers.AddRangeAsync(trainers.Select(TrainerConverter.CoreToDbModel));
            await _dbContextFixture.Context.SaveChangesAsync();

            var filter = new FilterTrainer(null, "Male", "fitness", 5, 20, null, null);

            // Act
            var result = await _trainerRepository.GetTrainerByFilterAsync(filter);

            // Assert
            Assert.Single(result);
            var filteredTrainer = result.First();
            Assert.Equal("John Doe", filteredTrainer.Name);
            Assert.Equal("Male", filteredTrainer.Gender);
            Assert.Equal("Fitness", filteredTrainer.Specialization);
            Assert.Equal(5, filteredTrainer.Experience);
            Assert.Equal(4, filteredTrainer.Rating);
        }

        [Fact]
        public async Task GetTrainerByIdAsync_Should_Return_Trainer_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var specialization = "Fitness";
            var experience = 5;
            var rating = 4;

            var trainer = new Trainer(id, userId, name, gender, specialization, experience, rating);

            await _dbContextFixture.Context.Trainers.AddAsync(TrainerConverter.CoreToDbModel(trainer));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _trainerRepository.GetTrainerByIdAsync(trainer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(trainer.Id, result.Id);
            Assert.Equal(trainer.Name, result.Name);
            Assert.Equal(trainer.Gender, result.Gender);
            Assert.Equal(trainer.Specialization, result.Specialization);
            Assert.Equal(trainer.Experience, result.Experience);
            Assert.Equal(trainer.Rating, result.Rating);
        }

        [Fact]
        public async Task GetTrainersAsync_Should_Return_List_Of_Trainers_From_Database()
        {
            // Arrange
            var trainers = new List<Trainer>
            {
                new Trainer(Guid.NewGuid(), Guid.NewGuid(), "John Doe", "Male", "Fitness", 5, 4),
                new Trainer(Guid.NewGuid(), Guid.NewGuid(), "Jane Smith", "Female", "Yoga", 3, 5),
                new Trainer(Guid.NewGuid(), Guid.NewGuid(), "Mark Johnson", "Male", "Crossfit", 7, 4),
            };

            await _dbContextFixture.Context.Trainers.AddRangeAsync(trainers.Select(TrainerConverter.CoreToDbModel));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _trainerRepository.GetTrainersAsync(null, null);

            // Assert
            Assert.Equal(trainers.Count, result.Count);
            for (int i = 0; i < trainers.Count; i++)
            {
                Assert.Equal(trainers[i].Name, result[i].Name);
                Assert.Equal(trainers[i].Gender, result[i].Gender);
                Assert.Equal(trainers[i].Specialization, result[i].Specialization);
                Assert.Equal(trainers[i].Experience, result[i].Experience);
                Assert.Equal(trainers[i].Rating, result[i].Rating);
            }
        }

        [Fact]
        public async Task GetTrainerNameByIdAsync_Should_Return_Trainer_Name_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var specialization = "Fitness";
            var experience = 5;
            var rating = 4;

            var trainer = new Trainer(id, userId, name, gender, specialization, experience, rating);

            await _dbContextFixture.Context.Trainers.AddAsync(TrainerConverter.CoreToDbModel(trainer));
            await _dbContextFixture.Context.SaveChangesAsync();

            // Act
            var result = await _trainerRepository.GetTrainerNameByIdAsync(trainer.Id);

            // Assert
            Assert.Equal(name, result);
        }

        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
