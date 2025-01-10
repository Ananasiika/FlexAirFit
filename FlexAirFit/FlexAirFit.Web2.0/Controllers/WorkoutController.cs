using System.ComponentModel.DataAnnotations;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Atributes;
using FlexAirFit.Web2._0.Dto.Converters;
using FlexAirFit.Web2._0.Dto.Dto;
using FlexAirFit.Web2._0.Helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlexAirFit.Web2._0.Controllers;

[ApiController]
[Route("/api/workouts")]
public class WorkoutController : ControllerBase
{
    private readonly IWorkoutService _workoutService;

    public WorkoutController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }
    
    /// <summary>
    /// Получение списка тренировок
    /// </summary>
    /// <response code="200">Тренировки успешно получены.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    /// <returns>Список тренировок.</returns>
    [HttpGet()]
    [SwaggerResponse(statusCode: 200,description: "Тренировки успешно получены.")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> GetWorkouts([FromQuery] WorkoutQueryParamsDto? filter, int? limit, int? offset)
    {
        try
        {
            var workouts = await _workoutService.GetWorkoutByFilter(WorkoutQueryParamsDtoConverter.ToCore(filter), limit, offset);

            return Ok(workouts.ConvertAll(WorkoutDtoConverter.ToDto));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    /// <summary>
    /// Создание новой тренировки
    /// </summary>
    /// <response code="201">Тренировка успешно создана</response>
    /// <response code="400">Неверные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    /// <returns>Добавленная тренировка.</returns>
    [HttpPost()]
    [Authorize(UserRole.Admin, UserRole.Trainer)]
    [SwaggerResponse(statusCode: 201,description: "Тренировка успешно создана")]
    [SwaggerResponse(statusCode: 400,description: "Неверные данные")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> AddWorkout([Required][FromBody] CreateWorkoutRequestDto workoutDto)
    {
        try
        {
            var workout = CreateWorkoutRequestDtoConverter.ToCore(workoutDto);
            await _workoutService.CreateWorkout(workout);

            return Created("/api/workouts/" + workout.Id, workout);
        }
        catch (CreateWorkoutRequestException)
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
    /// Обновление информации о тренировке
    /// </summary>
    /// <response code="200">Тренировка успешно обновлена</response>
    /// <response code="400">Ошибка в запросе</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Тренировка не найдена</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPut("{id}")]
    [Authorize(UserRole.Admin, UserRole.Trainer)]
    [SwaggerResponse(statusCode: 200, description: "Тренировка успешно обновлена")]
    [SwaggerResponse(statusCode: 400, description: "Ошибка в запросе")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Тренировка не найдена")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> UpdateWorkout([Required] Guid id, [FromBody] UpdateWorkoutRequestDto request)
    {
        try
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimNamesHelper.IdClaimName)?.Value;
            var roleClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimNamesHelper.RoleClaimName)?.Value;
            
            if (roleClaim == UserRole.Trainer.ToString() && request.IdTrainer.ToString() != userId)
                throw new UserForbiddenException();
            var updatedWorkout = await _workoutService.UpdateWorkout(UpdateWorkoutRequestDtoConverter.ToCore(request, id));
            return Ok(WorkoutDtoConverter.ToDto(updatedWorkout));
        }
        catch (WorkoutNotFoundException)
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
    /// Удаление тренировки
    /// </summary>
    /// <response code="204">Тренировка успешно удалена</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Тренировка не найдена</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpDelete("{id}")]
    [Authorize(UserRole.Admin, UserRole.Trainer)]
    [SwaggerResponse(statusCode: 204, description: "Тренировка успешно удалена")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Тренировка не найдена")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> DeleteWorkout([Required] Guid id)
    {
        try
        {
            var workout = await _workoutService.GetWorkoutById(id);
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimNamesHelper.IdClaimName)?.Value;
            var roleClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimNamesHelper.RoleClaimName)?.Value;
            
            if (roleClaim == UserRole.Trainer.ToString() && workout.IdTrainer.ToString() != userId)
                throw new UserForbiddenException();
            await _workoutService.DeleteWorkout(id);
            return NoContent();
        }
        catch (WorkoutNotFoundException)
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
    /// Получение названия тренировки
    /// </summary>
    /// <response code="200">Название тренировки успешно получено</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Тренировка не найдена</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{id}")]
    [Authorize(UserRole.Admin, UserRole.Trainer, UserRole.Client)]
    [SwaggerResponse(statusCode: 200, description: "Название тренировки успешно получено")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Тренировка не найдена")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> GetWorkoutNameById([Required] Guid id)
    {
        try
        {
            var name = await _workoutService.GetWorkoutNameById(id);
            return Ok(GetNameWorkoutResponseDtoConverter.ToDto(name));
        }
        catch (WorkoutNotFoundException)
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