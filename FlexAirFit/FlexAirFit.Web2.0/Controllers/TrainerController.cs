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
[Route("/api/trainers")]
public class TrainerController : ControllerBase
{
    private readonly ITrainerService _trainerService;

    public TrainerController(ITrainerService trainerService)
    {
        _trainerService = trainerService;
    }
    
    /// <summary>
    /// Получение списка тренеров
    /// </summary>
    /// <response code="200">Тренеры успешно получены.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    /// <returns>Список тренеров.</returns>
    [HttpGet()]
    [SwaggerResponse(statusCode: 200,description: "Тренеры успешно получены.")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> GetTrainers(int? limit, int? offset)
    {
        try
        {
            var trainers = await _trainerService.GetTrainers(limit, offset);

            return Ok(trainers.ConvertAll(TrainerDtoConverter.ToDto));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    /// <summary>
    /// Создание нового тренера
    /// </summary>
    /// <response code="201">Тренер успешно создан</response>
    /// <response code="400">Неверные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    /// <returns>Добавленный тренер.</returns>
    [HttpPost("{id}")]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 201,description: "Тренер успешно создан")]
    [SwaggerResponse(statusCode: 400,description: "Неверные данные")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> AddTrainer([Required] Guid id, [Required][FromBody] CreateTrainerRequestDto trainerDto)
    {
        try
        {
            var trainer = CreateTrainerRequestDtoConverter.ToCore(id, trainerDto);
            await _trainerService.CreateTrainer(trainer);

            return Created("/api/trainers/" + trainer.Id, trainer);
        }
        catch (CreateTrainerRequestException)
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
    /// Обновление информации о тренере
    /// </summary>
    /// <response code="200">Тренер успешно обновлен</response>
    /// <response code="400">Ошибка в запросе</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Тренер не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPut("{id}")]
    [Authorize(UserRole.Admin, UserRole.Trainer)]
    [SwaggerResponse(statusCode: 200, description: "Тренер успешно обновлен")]
    [SwaggerResponse(statusCode: 400, description: "Ошибка в запросе")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Тренер не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> UpdateTrainer([Required] Guid id, [FromBody] UpdateTrainerRequestDto request)
    {
        try
        {
            var updatedTrainer = await _trainerService.UpdateTrainer(UpdateTrainerRequestDtoConverter.ToCore(request, id));
            return Ok(TrainerDtoConverter.ToDto(updatedTrainer));
        }
        catch (TrainerNotFoundException)
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
    /// Удаление тренера
    /// </summary>
    /// <response code="204">Тренер успешно удален</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Тренер не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpDelete("{id}")]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 204, description: "Тренер успешно удален")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Тренер не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> DeleteTrainer([Required] Guid id)
    {
        try
        {
            await _trainerService.DeleteTrainer(id);
            return NoContent();
        }
        catch (TrainerNotFoundException)
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
    /// Получение имение тренера
    /// </summary>
    /// <response code="200">Имя тренера успешно получено</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Тренер не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{id}")]
    [Authorize(UserRole.Admin, UserRole.Client, UserRole.Trainer)]
    [SwaggerResponse(statusCode: 200, description: "Имя тренера успешно получено")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Тренер не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> GetTrainerNameById([Required] Guid id)
    {
        try
        {
            var name = await _trainerService.GetTrainerNameById(id);
            return Ok(GetNameTrainerResponseDtoConverter.ToDto(name));
        }
        catch (TrainerNotFoundException)
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