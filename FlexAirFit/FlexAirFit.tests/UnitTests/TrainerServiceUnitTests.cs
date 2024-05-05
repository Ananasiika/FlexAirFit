using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.Services;

namespace FlexAirFit.Tests;

public class TrainerServiceUnitTests
{
    private readonly Mock<ITrainerRepository> _mockTrainerRepository;
    private readonly ITrainerService _trainerService;

    public TrainerServiceUnitTests()
    {
        _mockTrainerRepository = new Mock<ITrainerRepository>();
        _trainerService = new TrainerService(_mockTrainerRepository.Object);
    }

    [Fact]
    public async Task CreateTrainer_ShouldCallAddTrainerAsync_WhenTrainerDoesNotExist()
    {
        var trainer = new Trainer(Guid.NewGuid(), "Sample Trainer", "Sample Gender", "Sample Specialization", 2, 4);

        _mockTrainerRepository.Setup(r => r.GetTrainerByIdAsync(trainer.Id)).ReturnsAsync((Trainer)null);

        await _trainerService.CreateTrainer(trainer);

        _mockTrainerRepository.Verify(r => r.AddTrainerAsync(trainer), Times.Once);
    }

    [Fact]
    public async Task CreateTrainer_ShouldThrowTrainerExistsException_WhenTrainerExists()
    {
        var trainer = new Trainer(Guid.NewGuid(), "Sample Trainer", "Sample Gender", "Sample Specialization", 2, 4);

        _mockTrainerRepository.Setup(r => r.GetTrainerByIdAsync(trainer.Id)).ReturnsAsync(trainer);

        await Assert.ThrowsAsync<TrainerExistsException>(() => _trainerService.CreateTrainer(trainer));
    }

    [Fact]
    public async Task UpdateTrainer_ShouldCallUpdateTrainerAsync_WhenTrainerExists()
    {
        var trainer = new Trainer(Guid.NewGuid(), "Sample Trainer", "Sample Gender", "Sample Specialization", 2, 4);

        _mockTrainerRepository.Setup(r => r.GetTrainerByIdAsync(trainer.Id)).ReturnsAsync(trainer);

        await _trainerService.UpdateTrainer(trainer);

        _mockTrainerRepository.Verify(r => r.UpdateTrainerAsync(trainer), Times.Once);
    }

    [Fact]
    public async Task UpdateTrainer_ShouldThrowTrainerNotFoundException_WhenTrainerDoesNotExist()
    {
        var trainer = new Trainer(Guid.NewGuid(), "Sample Trainer", "Sample Gender", "Sample Specialization", 2, 4);

        _mockTrainerRepository.Setup(r => r.GetTrainerByIdAsync(trainer.Id)).ReturnsAsync((Trainer)null);

        await Assert.ThrowsAsync<TrainerNotFoundException>(() => _trainerService.UpdateTrainer(trainer));
    }

    [Fact]
    public async Task DeleteTrainer_ShouldCallDeleteTrainerAsync_WhenTrainerExists()
    {
        var trainerId = Guid.NewGuid();

        _mockTrainerRepository.Setup(r => r.GetTrainerByIdAsync(trainerId)).ReturnsAsync(new Trainer(trainerId, "Sample Trainer", "Sample Gender", "Sample Specialization", 2, 4));

        await _trainerService.DeleteTrainer(trainerId);

        _mockTrainerRepository.Verify(r => r.DeleteTrainerAsync(trainerId), Times.Once);
    }

    [Fact]
    public async Task DeleteTrainer_ShouldThrowTrainerNotFoundException_WhenTrainerDoesNotExist()
    {
        var trainerId = Guid.NewGuid();

        _mockTrainerRepository.Setup(r => r.GetTrainerByIdAsync(trainerId)).ReturnsAsync((Trainer)null);

        await Assert.ThrowsAsync<TrainerNotFoundException>(() => _trainerService.DeleteTrainer(trainerId));
    }

    [Fact]
    public async Task GetTrainerById_ShouldReturnTrainer_WhenTrainerExists()
    {
        var trainerId = Guid.NewGuid();
        var trainer = new Trainer(trainerId, "Sample Trainer", "Sample Gender", "Sample Specialization", 2, 4);

        _mockTrainerRepository.Setup(r => r.GetTrainerByIdAsync(trainerId)).ReturnsAsync(trainer);

        var result = await _trainerService.GetTrainerById(trainerId);

        Assert.Equal(trainer, result);
    }

    [Fact]
    public async Task GetTrainerById_ShouldThrowTrainerNotFoundException_WhenTrainerDoesNotExist()
    {
        var trainerId = Guid.NewGuid();

        _mockTrainerRepository.Setup(r => r.GetTrainerByIdAsync(trainerId)).ReturnsAsync((Trainer)null);

        await Assert.ThrowsAsync<TrainerNotFoundException>(() => _trainerService.GetTrainerById(trainerId));
    }

    [Fact]
    public async Task GetTrainers_ShouldReturnListOfTrainers_WithLimitAndOffset()
    {
        var trainers = new List<Trainer>
        {
            new Trainer(Guid.NewGuid(), "Sample Trainer 1", "Sample Gender 1", "Sample Specialization 1", 2, 4),
            new Trainer(Guid.NewGuid(), "Sample Trainer 2", "Sample Gender 2", "Sample Specialization 2", 3, 5),
            new Trainer(Guid.NewGuid(), "Sample Trainer 3", "Sample Gender 3", "Sample Specialization 3", 4, 6)
        };

        _mockTrainerRepository.Setup(r => r.GetTrainersAsync(2, 1)).ReturnsAsync(trainers);

        var result = await _trainerService.GetTrainers(2, 1);

        Assert.Equal(trainers, result);
    }

    [Fact]
    public async Task GetTrainerNameById_ShouldReturnTrainerName_WhenTrainerExists()
    {
        var trainerId = Guid.NewGuid();
        var trainerName = "Sample Trainer";

        _mockTrainerRepository.Setup(r => r.GetTrainerNameByIdAsync(trainerId)).ReturnsAsync(trainerName);

        var result = await _trainerService.GetTrainerNameById(trainerId);

        Assert.Equal(trainerName, result);
    }

    [Fact]
    public async Task GetTrainerNameById_ShouldThrowTrainerNotFoundException_WhenTrainerDoesNotExist()
    {
        var trainerId = Guid.NewGuid();

        _mockTrainerRepository.Setup(r => r.GetTrainerNameByIdAsync(trainerId)).ReturnsAsync((string)null);

        await Assert.ThrowsAsync<TrainerNotFoundException>(() => _trainerService.GetTrainerNameById(trainerId));
    }
}
