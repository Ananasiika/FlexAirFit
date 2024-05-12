using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.Services;

namespace FlexAirFit.Tests;

public class ScheduleServiceUnitTests
{
    private readonly Mock<IScheduleRepository> _mockScheduleRepository;
    private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly IScheduleService _scheduleService;

    public ScheduleServiceUnitTests()
    {
        _mockScheduleRepository = new Mock<IScheduleRepository>();
        _mockClientRepository = new Mock<IClientRepository>();
        _mockWorkoutRepository = new Mock<IWorkoutRepository>();
        _scheduleService = new ScheduleService(_mockScheduleRepository.Object, _mockWorkoutRepository.Object, _mockClientRepository.Object);
    }

    [Fact]
    public async Task CreateSchedule_ShouldThrowScheduleExistsException_WhenScheduleExists()
    {
        var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(schedule.Id)).ReturnsAsync(schedule);

        await Assert.ThrowsAsync<ScheduleExistsException>(() => _scheduleService.CreateSchedule(schedule));
    }
    
    [Fact]
    public async Task CreateSchedule_ShouldThrowWorkoutNotFoundException_WhenWorkoutDoesNotExist()
    {
        // Arrange
        var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());
        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(schedule.Id)).ReturnsAsync((Schedule)null);
        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(schedule.IdWorkout)).ReturnsAsync((Workout)null);

        // Act & Assert
        await Assert.ThrowsAsync<WorkoutNotFoundException>(() => _scheduleService.CreateSchedule(schedule));
    }
    
    [Fact]
    public async Task CreateSchedule_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
    {
        // Arrange
        var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());
        var workout = new Workout(schedule.IdWorkout, "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);
        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(schedule.Id)).ReturnsAsync((Schedule)null);
        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(schedule.IdWorkout)).ReturnsAsync(workout);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync((Guid)schedule.IdClient)).ReturnsAsync((Client)null);

        // Act & Assert
        await Assert.ThrowsAsync<ClientNotFoundException>(() => _scheduleService.CreateSchedule(schedule));
    }

    [Fact]
    public async Task CreateSchedule_ShouldThrowScheduleTimeIncorrectedException_WhenScheduleTimeIsIncorrect()
    {
        // Arrange
        var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Parse("2023-04-21 23:00:00"), Guid.NewGuid());
        var workout = new Workout(schedule.IdWorkout, "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);
        var client = new Client((Guid)schedule.IdClient, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);
        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(schedule.Id)).ReturnsAsync((Schedule)null);
        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(schedule.IdWorkout)).ReturnsAsync(workout);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync((Guid)schedule.IdClient)).ReturnsAsync(client);

        // Act & Assert
        await Assert.ThrowsAsync<ScheduleTimeIncorrectedException>(() => _scheduleService.CreateSchedule(schedule));
    }
    
    [Fact]
    public async Task CreateSchedule_ShouldCallAddScheduleAsync_WhenScheduleIsValid()
    {
        // Arrange
        var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Parse("2024-06-06 21:00:00"), Guid.NewGuid());
        var workout = new Workout(schedule.IdWorkout, "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);
        var client = new Client((Guid)schedule.IdClient, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);
        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(schedule.Id)).ReturnsAsync((Schedule)null);
        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(schedule.IdWorkout)).ReturnsAsync(workout);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync((Guid)schedule.IdClient)).ReturnsAsync(client);

        // Act
        await _scheduleService.CreateSchedule(schedule);

        // Assert
        _mockScheduleRepository.Verify(r => r.AddScheduleAsync(schedule), Times.Once);
    }


    [Fact]
    public async Task CreateSchedule_ShouldThrowClientAlreadyHasScheduleException_WhenClientAlreadyHasSchedule()
    {
        // Arrange
        var existingSchedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Parse("2024-06-06 21:00:00"), Guid.NewGuid());
        var workout = new Workout(existingSchedule.IdWorkout, "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);
        var client = new Client((Guid)existingSchedule.IdClient, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);
        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(existingSchedule.Id)).ReturnsAsync((Schedule)null);
        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(existingSchedule.IdWorkout)).ReturnsAsync(workout);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync((Guid)existingSchedule.IdClient)).ReturnsAsync(client);
        
        var schedule = new Schedule(Guid.NewGuid(), existingSchedule.IdWorkout, DateTime.Parse("2024-06-06 21:00:00"), client.Id);

        // Act & Assert
        await Assert.ThrowsAsync<ClientAlreadyHasScheduleException>(() => _scheduleService.CreateSchedule(schedule));
    }

    [Fact]
    public async Task CreateSchedule_ShouldThrowTrainerAlreadyHasScheduleException_WhenTrainerAlreadyHasSchedule()
    {
        // Arrange
        var existingSchedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Parse("2024-06-06 21:00:00"), Guid.NewGuid());
        var workout = new Workout(existingSchedule.IdWorkout, "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 5);
        var client = new Client((Guid)existingSchedule.IdClient, "John Doe", "Male", DateTime.Today, Guid.NewGuid(), DateTime.Today, 10, null);
        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(existingSchedule.Id)).ReturnsAsync((Schedule)null);
        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(existingSchedule.IdWorkout)).ReturnsAsync(workout);
        _mockClientRepository.Setup(r => r.GetClientByIdAsync((Guid)existingSchedule.IdClient)).ReturnsAsync(client);
        
        var schedule = new Schedule(Guid.NewGuid(), existingSchedule.IdWorkout, DateTime.Parse("2024-06-06 21:30:00"), null);

        // Act & Assert
        await Assert.ThrowsAsync<TrainerAlreadyHasScheduleException>(() => _scheduleService.CreateSchedule(schedule));
    }
    
    [Fact]
    public async Task UpdateSchedule_ShouldCallUpdateScheduleAsync_WhenScheduleExists()
    {
        var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(schedule.Id)).ReturnsAsync(schedule);

        await _scheduleService.UpdateSchedule(schedule);

        _mockScheduleRepository.Verify(r => r.UpdateScheduleAsync(schedule), Times.Once);
    }

    [Fact]
    public async Task UpdateSchedule_ShouldThrowScheduleNotFoundException_WhenScheduleDoesNotExist()
    {
        var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(schedule.Id)).ReturnsAsync((Schedule)null);

        await Assert.ThrowsAsync<ScheduleNotFoundException>(() => _scheduleService.UpdateSchedule(schedule));
    }

    [Fact]
    public async Task DeleteSchedule_ShouldCallDeleteScheduleAsync_WhenScheduleExists()
    {
        var scheduleId = Guid.NewGuid();

        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(scheduleId)).ReturnsAsync(new Schedule(scheduleId, Guid.NewGuid(), DateTime.Now, Guid.NewGuid()));

        await _scheduleService.DeleteSchedule(scheduleId);

        _mockScheduleRepository.Verify(r => r.DeleteScheduleAsync(scheduleId), Times.Once);
    }

    [Fact]
    public async Task DeleteSchedule_ShouldThrowScheduleNotFoundException_WhenScheduleDoesNotExist()
    {
        var scheduleId = Guid.NewGuid();

        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(scheduleId)).ReturnsAsync((Schedule)null);

        await Assert.ThrowsAsync<ScheduleNotFoundException>(() => _scheduleService.DeleteSchedule(scheduleId));
    }

    [Fact]
    public async Task GetScheduleById_ShouldReturnSchedule_WhenScheduleExists()
    {
        var scheduleId = Guid.NewGuid();
        var schedule = new Schedule(scheduleId, Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(scheduleId)).ReturnsAsync(schedule);

        var result = await _scheduleService.GetScheduleById(scheduleId);

        Assert.Equal(schedule, result);
    }

    [Fact]
    public async Task GetScheduleById_ShouldThrowScheduleNotFoundException_WhenScheduleDoesNotExist()
    {
        var scheduleId = Guid.NewGuid();

        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(scheduleId)).ReturnsAsync((Schedule)null);

        await Assert.ThrowsAsync<ScheduleNotFoundException>(() => _scheduleService.GetScheduleById(scheduleId));
    }

    [Fact]
    public async Task GetSchedules_ShouldReturnListOfSchedules_WithLimitAndOffset()
    {
        var schedules = new List<Schedule>
        {
            new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid()),
            new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid()),
            new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid())
        };

        _mockScheduleRepository.Setup(r => r.GetSchedulesAsync(2, 1)).ReturnsAsync(schedules);

        var result = await _scheduleService.GetSchedules(2, 1);

        Assert.Equal(schedules, result);
    }
}
