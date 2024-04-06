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

public class ScheduleServiceUnitTests
{
    private readonly Mock<IScheduleRepository> _mockScheduleRepository;
    private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
    private readonly IScheduleService _scheduleService;

    public ScheduleServiceUnitTests()
    {
        _mockScheduleRepository = new Mock<IScheduleRepository>();
        _mockWorkoutRepository = new Mock<IWorkoutRepository>();
        _scheduleService = new ScheduleService(_mockScheduleRepository.Object, _mockWorkoutRepository.Object);
    }

    [Fact]
    public async Task CreateSchedule_ShouldCallAddScheduleAsync_WhenScheduleDoesNotExist()
    {
        var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(schedule.Id)).ReturnsAsync((Schedule)null);

        await _scheduleService.CreateSchedule(schedule);

        _mockScheduleRepository.Verify(r => r.AddScheduleAsync(schedule), Times.Once);
    }

    [Fact]
    public async Task CreateSchedule_ShouldThrowScheduleExistsException_WhenScheduleExists()
    {
        var schedule = new Schedule(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Guid.NewGuid());

        _mockScheduleRepository.Setup(r => r.GetScheduleByIdAsync(schedule.Id)).ReturnsAsync(schedule);

        await Assert.ThrowsAsync<ScheduleExistsException>(() => _scheduleService.CreateSchedule(schedule));
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

