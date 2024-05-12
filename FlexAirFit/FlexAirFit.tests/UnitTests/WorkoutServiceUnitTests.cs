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

namespace FlexAirFit.Tests;

public class WorkoutServiceUnitTests
{
    private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
    private readonly Mock<ITrainerRepository> _mockTrainerRepository;
    private readonly IWorkoutService _workoutService;

    public WorkoutServiceUnitTests()
    {
        _mockWorkoutRepository = new Mock<IWorkoutRepository>();
        _mockTrainerRepository = new Mock<ITrainerRepository>();
        _workoutService = new WorkoutService(_mockWorkoutRepository.Object, _mockTrainerRepository.Object);
    }

    [Fact]
    public async Task CreateWorkout_ShouldCallAddWorkoutAsync_WhenWorkoutDoesNotExist()
    {
        var workout = new Workout(Guid.NewGuid(), "Workout Name", "Workout Description", Guid.NewGuid(), TimeSpan.FromMinutes(60), 1);

        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(workout.Id)).ReturnsAsync((Workout)null);

        await _workoutService.CreateWorkout(workout);

        _mockWorkoutRepository.Verify(r => r.AddWorkoutAsync(workout), Times.Once);
    }

    [Fact]
    public async Task CreateWorkout_ShouldThrowWorkoutExistsException_WhenWorkoutExists()
    {
        var workout = new Workout(Guid.NewGuid(), "Workout Name", "Workout Description", Guid.NewGuid(), TimeSpan.FromMinutes(60), 1);

        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(workout.Id)).ReturnsAsync(workout);

        await Assert.ThrowsAsync<WorkoutExistsException>(() => _workoutService.CreateWorkout(workout));
    }

    [Fact]
    public async Task UpdateWorkout_ShouldCallUpdateWorkoutAsync_WhenWorkoutExists()
    {
        var workout = new Workout(Guid.NewGuid(), "Workout Name", "Workout Description", Guid.NewGuid(), TimeSpan.FromMinutes(60), 1);

        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(workout.Id)).ReturnsAsync(workout);

        await _workoutService.UpdateWorkout(workout);

        _mockWorkoutRepository.Verify(r => r.UpdateWorkoutAsync(workout), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkout_ShouldThrowWorkoutNotFoundException_WhenWorkoutDoesNotExist()
    {
        var workout = new Workout(Guid.NewGuid(), "Workout Name", "Workout Description", Guid.NewGuid(), TimeSpan.FromMinutes(60), 1);

        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(workout.Id)).ReturnsAsync((Workout)null);

        await Assert.ThrowsAsync<WorkoutNotFoundException>(() => _workoutService.UpdateWorkout(workout));
    }

    [Fact]
    public async Task DeleteWorkout_ShouldThrowWorkoutNotFoundException_WhenWorkoutDoesNotExist()
    {
        var workoutId = Guid.NewGuid();

        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(workoutId)).ReturnsAsync((Workout)null);

        await Assert.ThrowsAsync<WorkoutNotFoundException>(() => _workoutService.DeleteWorkout(workoutId));
    }

    [Fact]
    public async Task GetWorkoutByFilter_ShouldReturnListOfWorkouts()
    {
        var filter = new FilterWorkout("Workout Name", null, null, null, null, null);
        var workouts = new List<Workout>
        {
            new Workout(Guid.NewGuid(), "Workout 1", "Workout Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 1),
            new Workout(Guid.NewGuid(), "Workout 2", "Workout Description 2", Guid.NewGuid(), TimeSpan.FromMinutes(45), 2)
        };

        _mockWorkoutRepository.Setup(r => r.GetWorkoutByFilterAsync(filter, null, null)).ReturnsAsync(workouts);

        var result = await _workoutService.GetWorkoutByFilter(filter, null, null);

        Assert.Equal(workouts, result);
    }

    [Fact]
    public async Task GetWorkoutById_ShouldReturnWorkout_WhenWorkoutExists()
    {
        var workoutId = Guid.NewGuid();
        var workout = new Workout(workoutId, "Workout Name", "Workout Description", Guid.NewGuid(), TimeSpan.FromMinutes(60), 1);

        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(workoutId)).ReturnsAsync(workout);

        var result = await _workoutService.GetWorkoutById(workoutId);

        Assert.Equal(workout, result);
    }

    [Fact]
    public async Task GetWorkoutById_ShouldThrowWorkoutNotFoundException_WhenWorkoutDoesNotExist()
    {
        var workoutId = Guid.NewGuid();

        _mockWorkoutRepository.Setup(r => r.GetWorkoutByIdAsync(workoutId)).ReturnsAsync((Workout)null);

        await Assert.ThrowsAsync<WorkoutNotFoundException>(() => _workoutService.GetWorkoutById(workoutId));
    }

    [Fact]
    public async Task GetWorkouts_ShouldReturnListOfWorkouts_WithLimitAndOffset()
    {
        var workouts = new List<Workout>
        {
            new Workout(Guid.NewGuid(), "Workout 1", "Workout Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 1),
            new Workout(Guid.NewGuid(), "Workout 2", "Workout Description 2", Guid.NewGuid(), TimeSpan.FromMinutes(45), 2)
        };

        _mockWorkoutRepository.Setup(r => r.GetWorkoutsAsync(2, 1)).ReturnsAsync(workouts);

        var result = await _workoutService.GetWorkouts(2, 1);

        Assert.Equal(workouts, result);
    }

    [Fact]
    public async Task GetWorkoutNameById_ShouldReturnWorkoutName_WhenWorkoutExists()
    {
        var workoutId = Guid.NewGuid();
        var workoutName = "Workout Name";

        _mockWorkoutRepository.Setup(r => r.GetWorkoutNameByIdAsync(workoutId)).ReturnsAsync(workoutName);

        var result = await _workoutService.GetWorkoutNameById(workoutId);

        Assert.Equal(workoutName, result);
    }

    [Fact]
    public async Task GetWorkoutNameById_ShouldThrowWorkoutNotFoundException_WhenWorkoutDoesNotExist()
    {
        var workoutId = Guid.NewGuid();

        _mockWorkoutRepository.Setup(r => r.GetWorkoutNameByIdAsync(workoutId)).ReturnsAsync((string)null);

        await Assert.ThrowsAsync<WorkoutNotFoundException>(() => _workoutService.GetWorkoutNameById(workoutId));
    }
}
