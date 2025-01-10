using System.ComponentModel.DataAnnotations;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Atributes;
using FlexAirFit.Web2._0.Dto.Converters;
using FlexAirFit.Web2._0.Dto.Converters.Bonus;
using FlexAirFit.Web2._0.Dto.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlexAirFit.Web2._0.Controllers;

[ApiController]
[Route("/api/bonuses")]
public class BonusController : ControllerBase
{
    private readonly IBonusService _bonusService;

    public BonusController(IBonusService bonusService)
    {
        _bonusService = bonusService;
    }
    
    /// <summary>
    /// Получение количества бонусов клиента
    /// </summary>
    /// <response code="200">Количества бонусов клиента успешно получено</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="400">Клиент не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{id}")]
    [Authorize(UserRole.Admin, UserRole.Client)]
    [SwaggerResponse(statusCode: 200, description: "Количества бонусов клиента успешно получено")]
    [SwaggerResponse(statusCode: 404, description: "Клиент не найден")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> GetClientBonuses([Required] Guid id)
    {
        try
        {
            var bonuses = await _bonusService.GetCountBonusByIdClient(id);
            return Ok(BonusDtoConverter.ToDto(bonuses));
        }
        catch (ClientNotFoundException)
        {
            return StatusCode(404);
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