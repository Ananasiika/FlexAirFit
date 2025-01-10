using System.ComponentModel.DataAnnotations;
using FlexAirFit.Application.Exceptions.ServiceException;
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
[Route("/api/memberships")]
public class MembershipController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    public MembershipController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }
    
    /// <summary>
    /// Получение списка абонементов
    /// </summary>
    /// <response code="200">Абонементы успешно получены.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    /// <returns>Список абонементов.</returns>
    [HttpGet()]
    [SwaggerResponse(statusCode: 200,description: "Абонементы успешно получены.")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> GetMemberships(int? limit, int? offset)
    {
        try
        {
            var memberships = await _membershipService.GetMemberships(limit, offset);

            return Ok(memberships.ConvertAll(MembershipDtoConverter.ToDto));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    /// <summary>
    /// Создание нового абонемента
    /// </summary>
    /// <response code="201">Абонемент успешно создан</response>
    /// <response code="400">Неверные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    /// <returns>Добавленный абонемент.</returns>
    [HttpPost()]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 201,description: "Абонемент успешно создан")]
    [SwaggerResponse(statusCode: 400,description: "Неверные данные")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> AddMembership([Required][FromBody] CreateMembershipRequestDto membershipDto)
    {
        try
        {
            var membership = CreateMembershipRequestDtoConverter.ToCore(membershipDto);
            await _membershipService.CreateMembership(membership);

            return Created("/api/memberships/" + membership.Id, membership);
        }
        catch (CreateMembershipRequestException)
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
    /// Обновление информации об абонементе
    /// </summary>
    /// <response code="200">Абонемент успешно обновлен</response>
    /// <response code="400">Ошибка в запросе</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Абонемент не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPut("{id}")]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 200, description: "Абонемент успешно обновлен")]
    [SwaggerResponse(statusCode: 400, description: "Ошибка в запросе")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Абонемент не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> UpdateMembership([Required] Guid id, [FromBody] UpdateMembershipRequestDto request)
    {
        try
        {
            var updatedMembership = await _membershipService.UpdateMembership(UpdateMembershipRequestDtoConverter.ToCore(request, id));
            return Ok(MembershipDtoConverter.ToDto(updatedMembership));
        }
        catch (MembershipNotFoundException)
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
    /// Удаление абонемента
    /// </summary>
    /// <response code="204">Абонемент успешно удален</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Абонемент не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpDelete("{id}")]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 204, description: "Абонемент успешно удален")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Абонемент не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> DeleteMembership([Required] Guid id)
    {
        try
        {
            await _membershipService.DeleteMembership(id);
            return NoContent();
        }
        catch (MembershipNotFoundException)
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