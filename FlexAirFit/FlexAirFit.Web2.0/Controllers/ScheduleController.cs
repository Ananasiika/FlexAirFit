using System.ComponentModel.DataAnnotations;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Atributes;
using FlexAirFit.Web2._0.Dto.Converters;
using FlexAirFit.Web2._0.Dto.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlexAirFit.Web2._0.Controllers;

[ApiController]
[Route("/api/schedule")]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }
    
    /// <summary>
    /// Получение расписания
    /// </summary>
    /// <response code="200">Расписание успешно получено.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    /// <returns>Расписание.</returns>
    [HttpGet()]
    [SwaggerResponse(statusCode: 200,description: "Расписание успешно получено.")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> GetSchedules([FromQuery] ScheduleQueryParamsDto? filter, int? limit, int? offset)
    {
        try
        {
            var schedules = await _scheduleService.GetScheduleByFilter(ScheduleQueryParamsDtoConverter.ToCore(filter), limit, offset);

            return Ok(schedules.ConvertAll(ScheduleDtoConverter.ToDto));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    /// <summary>
    /// Создание новой записи в расписании
    /// </summary>
    /// <response code="201">Запись успешно создана</response>
    /// <response code="400">Неверные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    /// <returns>Добавленная запись.</returns>
    [HttpPost()]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 201,description: "Запись успешно создана")]
    [SwaggerResponse(statusCode: 400,description: "Неверные данные")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> AddSchedule([Required][FromBody] CreateScheduleRequestDto scheduleDto)
    {
        try
        {
            var schedule = CreateScheduleRequestDtoConverter.ToCore(scheduleDto);
            await _scheduleService.CreateSchedule(schedule);

            return Created("/api/schedule/" + schedule.Id, schedule);
        }
        catch (CreateScheduleRequestException)
        {
            return StatusCode(400);
        }
        catch (UserNotAuthorizedException)
        {
            return StatusCode(401);
        }
        catch (UserForbiddenException)
        {
            return StatusCode(403);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    /// <summary>
    /// Обновление информации о записи в расписании
    /// </summary>
    /// <response code="200">Запись успешно обновлена</response>
    /// <response code="400">Ошибка в запросе</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Запись не найдена</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPut("{id}")]
    [Authorize(UserRole.Admin, UserRole.Trainer)]
    [SwaggerResponse(statusCode: 200, description: "Запись успешно обновлена")]
    [SwaggerResponse(statusCode: 400, description: "Ошибка в запросе")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Запись не найдена")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> UpdateSchedule([Required] Guid id, [FromBody] UpdateScheduleRequestDto request)
    {
        try
        {
            var updatedSchedule = await _scheduleService.UpdateSchedule(UpdateScheduleRequestDtoConverter.ToCore(request, id));
            return Ok(ScheduleDtoConverter.ToDto(updatedSchedule));
        }
        catch (ScheduleNotFoundException)
        {
            return StatusCode(400);
        }
        catch (UserNotAuthorizedException)
        {
            return StatusCode(401);
        }
        catch (UserForbiddenException)
        {
            return StatusCode(403);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}