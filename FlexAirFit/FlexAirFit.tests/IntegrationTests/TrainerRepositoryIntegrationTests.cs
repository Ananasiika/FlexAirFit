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
    public class TrainerServiceTests : IDisposable
    {
        private readonly InMemoryDbFixture _dbContextFixture = new InMemoryDbFixture();
        private readonly ITrainerService _trainerService;

        public TrainerServiceTests()
        {
            _trainerService = new TrainerService(new TrainerRepository(_dbContextFixture.Context));
        }

        [Fact]
        public async Task CreateTrainer_Should_Add_Trainer_To_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var specialization = "Fitness";
            var experience = 5;
            var rating = 4;

            var trainer = new Trainer(id, name, gender, specialization, experience, rating);

            // Act
            await _trainerService.CreateTrainer(trainer);

            // Assert
            var trainerDbModel = await _dbContextFixture.Context.Trainers.FindAsync(trainer.Id);
            Assert.NotNull(trainerDbModel);
            Assert.Equal(trainer.Name, trainerDbModel.Name);
            Assert.Equal(trainer.Gender, trainerDbModel.Gender);
            Assert.Equal(trainer.Specialization, trainerDbModel.Specialization);
            Assert.Equal(trainer.Experience, trainerDbModel.Experience);
            Assert.Equal(trainer.Rating, trainerDbModel.Rating);
        }

        [Fact]
        public async Task UpdateTrainer_Should_Update_Trainer_In_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var specialization = "Fitness";
            var experience = 5;
            var rating = 4;

            var trainer = new Trainer(id, name, gender, specialization, experience, rating);

            await _trainerService.CreateTrainer(trainer);

            
            trainer.Name = "Jane Smith";
            trainer.Rating = 5;

            // Act
            var updatedTrainer = await _trainerService.UpdateTrainer(trainer);

            // Assert
            Assert.NotNull(updatedTrainer);
            Assert.Equal(trainer.Id, updatedTrainer.Id);
            Assert.Equal(trainer.Name, updatedTrainer.Name);
            Assert.Equal(trainer.Gender, updatedTrainer.Gender); // Gender is not supposed to be updated in this test
            Assert.Equal(trainer.Specialization, updatedTrainer.Specialization); // Specialization is not supposed to be updated in this test
            Assert.Equal(trainer.Experience, updatedTrainer.Experience); // Experience is not supposed to be updated in this test
            Assert.Equal(trainer.Rating, updatedTrainer.Rating);
        }
        
        [Fact]
        public async Task GetTrainerById_Should_Return_Trainer_From_Database()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "John Doe";
            var gender = "Male";
            var specialization = "Fitness";
            var experience = 5;
            var rating = 4;

            var trainer = new Trainer(id, name, gender, specialization, experience, rating);
            await _trainerService.CreateTrainer(trainer);

            // Act
            var retrievedTrainer = await _trainerService.GetTrainerById(id);

            // Assert
            Assert.NotNull(retrievedTrainer);
            Assert.Equal(trainer.Id, retrievedTrainer.Id);
            Assert.Equal(trainer.Name, retrievedTrainer.Name);
            Assert.Equal(trainer.Gender, retrievedTrainer.Gender);
            Assert.Equal(trainer.Specialization, retrievedTrainer.Specialization);
            Assert.Equal(trainer.Experience, retrievedTrainer.Experience);
            Assert.Equal(trainer.Rating, retrievedTrainer.Rating);
        }

        [Fact]
        public async Task GetTrainerByFilter_Should_Return_Filtered_Trainers_From_Database()
        {
            // Arrange
            var filter = new FilterTrainer("John Doe", "Male", "Fitness", 3, 10, 3, 5);

            var trainers = new List<Trainer>
            {
                new Trainer(Guid.NewGuid(), "John Doe", "Male", "Fitness", 5, 4),
                new Trainer(Guid.NewGuid(), "Jane Smith", "Female", "Yoga", 4, 5),
                new Trainer(Guid.NewGuid(), "Jack Brown", "Male", "CrossFit", 6, 3),
            };

            await _trainerService.CreateTrainer(trainers[0]);
            await _trainerService.CreateTrainer(trainers[1]);
            await _trainerService.CreateTrainer(trainers[2]);
            // Act
            var result = await _trainerService.GetTrainerByFilter(filter, null, null);

            // Assert
            Assert.Single(result); // Only one trainer should match the filter criteria
            var filteredTrainer = result.First();
            Assert.Equal("John Doe", filteredTrainer.Name);
            Assert.Equal("Male", filteredTrainer.Gender);
            Assert.Equal("Fitness", filteredTrainer.Specialization);
            Assert.True(filteredTrainer.Experience >= 3 && filteredTrainer.Experience <= 6);
            Assert.True(filteredTrainer.Rating <= 5);
        }


        public void Dispose()
        {
            _dbContextFixture.Dispose();
        }
    }
}
