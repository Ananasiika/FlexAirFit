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
[Route("/api/clients")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IMembershipService _membershipService;

    public ClientController(IClientService clientService, IMembershipService membershipService)
    {
        _clientService = clientService;
        _membershipService = membershipService;
    }

    /// <summary>
    /// Получение списка клиентов
    /// </summary>
    /// <response code="200">Клиенты успешно получены.</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    /// <returns>Список клиентов.</returns>
    [HttpGet()]
    [Authorize(UserRole.Admin, UserRole.Trainer)]
    [SwaggerResponse(statusCode: 200, description: "Клиенты успешно получены.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> GetClients(int? limit, int? offset)
    {
        try
        {
            var clients = await _clientService.GetClients(limit, offset);

            return Ok(clients.ConvertAll(ClientDtoConverter.ToDto));
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
    /// Создание нового клиента
    /// </summary>
    /// <response code="201">Клиент успешно создан</response>
    /// <response code="400">Неверные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="409">Клиент с таким идентификатором уже существует</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    /// <returns>Добавленный клиент.</returns>
    [HttpPost("{id}")]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 201, description: "Клиент успешно создан")]
    [SwaggerResponse(statusCode: 400, description: "Неверные данные")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 409, description: "Клиент с таким идентификатором уже существует")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> AddClient([Required] Guid id,
        [Required] [FromBody] CreateClientRequestDto clientDto)
    {
        try
        {
            var membership = await _membershipService.GetMembershipById(clientDto.IdMembership);
            var client = CreateClientRequestDtoConverter.ToCore(id, clientDto, membership.Duration, membership.Freezing);
            await _clientService.CreateClient(client);

            return Created("/api/clients/" + client.Id, client);
        }
        catch (CreateClientRequestException)
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
        catch (ClientExistsException)
        {
            return StatusCode(409);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Обновление информации о клиенте
    /// </summary>
    /// <response code="200">Клиент успешно обновлен</response>
    /// <response code="400">Ошибка в запросе</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Клиент не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPatch("{id}")]
    [Authorize(UserRole.Admin, UserRole.Client)]
    [SwaggerResponse(statusCode: 200, description: "Клиент успешно обновлен")]
    [SwaggerResponse(statusCode: 400, description: "Ошибка в запросе")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Клиент не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> UpdateClient([Required] Guid id, [FromBody] UpdateClientRequestDto request)
    {
        try
        {
            var client = await _clientService.GetClientById(id);
            var updatedClient = await _clientService.UpdateClient(UpdateClientRequestDtoConverter.ToCore(request, client));
            return Ok(ClientDtoConverter.ToDto(updatedClient));
        }
        catch (ClientNotFoundException)
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
    /// Получение информации о клиенте
    /// </summary>
    /// <response code="200">Клиент успешно получен</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Клиент не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{id}")]
    [Authorize(UserRole.Admin, UserRole.Client, UserRole.Trainer)]
    [SwaggerResponse(statusCode: 200, description: "Клиент успешно получен")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Клиент не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> GetClient([Required] Guid id)
    {
        try
        {
            var client = await _clientService.GetClientById(id);
            return Ok(ClientDtoConverter.ToDto(client));
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
    
    /// <summary>
    /// Заморозка абонемента клиента
    /// </summary>
    /// <response code="200">Абонемент успешно заморожен</response>
    /// <response code="400">Ошибка в запросе</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Клиент не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPost("{id}/freeze")]
    [Authorize(UserRole.Admin, UserRole.Client)]
    [SwaggerResponse(statusCode: 200, description: "Абонемент успешно заморожен")]
    [SwaggerResponse(statusCode: 400, description: "Ошибка в запросе")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Клиент не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> FreezeClient([Required] Guid id, [Required][FromBody] FreezingMembershipRequestDto dto)
    {
        try
        {
            await _clientService.FreezeMembership(id, dto.StartDate, dto.Duration);
            
            return Ok();
        }
        catch (InvalidFreezingException)
        {
            return StatusCode(400);
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