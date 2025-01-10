using System.ComponentModel.DataAnnotations;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Atributes;
using FlexAirFit.Web2._0.Auth;
using FlexAirFit.Web2._0.Dto.Converters.User;
using FlexAirFit.Web2._0.Dto.Dto.Auth;
using FlexAirFit.Web2._0.Dto.Dto.User;
using FlexAirFit.Web2._0.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace FlexAirFit.Web2._0.Controllers;

[ApiController]
[Route("/api/auth")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IOptions<JwtConfiguration> _jwtConfiguration;

    public UserController(IUserService userService, IOptions<JwtConfiguration> jwtConfiguration)
    {
        _userService = userService;
        _jwtConfiguration = jwtConfiguration;
    }
    
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <response code="200">Успешная авторизация</response>
    /// <response code="401">Неверный логин или пароль</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPost("/api/auth/login")]
    [SwaggerResponse(statusCode: 200, description: "Успешная авторизация")]
    [SwaggerResponse(statusCode: 401, description: "Неверный логин или пароль")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> Login([Required] LoginDto dto)
    {
        try
        {
            var user = await _userService.SignInUser(dto.Email, dto.Password);
            var response = CreateResponse(user, _jwtConfiguration.Value);
            return Ok(response);
        }
        catch (UserNotAuthorizedException)
        {
            return StatusCode(401);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <response code="201">Успешная регистрация</response>
    /// <response code="400">Ошибка в запросе</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPost("/api/auth/register")]
    [SwaggerResponse(statusCode: 201, description: "Успешная регистрация")]
    [SwaggerResponse(statusCode: 400, description: "Ошибка в запросе")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> Register([Required] RegisterDto dto)
    {
        try
        {
            var user = RegisterDtoConverter.ToCore(dto);
            await _userService.CreateUser(user, dto.Password, dto.Role);
            return Created("/api/auth/register", RegisterDtoConverter.ToDto(user));
        }
        catch (UserNotFoundException)
        {
            return StatusCode(400);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    /// <summary>
    /// Изменение пароля пользователя
    /// </summary>
    /// <response code="200">Пароль успешно изменен</response>
    /// <response code="400">Неверные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Пользователь не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPatch("{id}")]
    [Authorize(UserRole.Admin, UserRole.Client, UserRole.Trainer)]
    [SwaggerResponse(statusCode: 200, description: "Пароль успешно изменен")]
    [SwaggerResponse(statusCode: 400,description: "Неверные данные")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Пользователь не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> ChangePassword([Required]  Guid id, [Required] ChangePasswordRequestDto dto)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            await _userService.UpdatePasswordUser(user, dto.NewPassword);
            return Ok();
        }
        catch (UserNotAuthorizedException)
        {
            return StatusCode(400);
        }
        catch (UserNotCorrectPasswordException)
        {
            return StatusCode(401);
        }
        catch (UserForbiddenException)
        {
            return StatusCode(403);
        }
        catch (UserNotFoundException)
        {
            return StatusCode(404);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    private AuthorizationResult CreateResponse(User user, JwtConfiguration jwtConfig)
    {
        var identity = JwtHelper.CreateClaimsIdentity(user);
    
        var encodedJwt = JwtHelper.CreateJwtToken(identity.Claims, jwtConfig.Issuer, jwtConfig.Audience, jwtConfig.SecretKey, jwtConfig.Lifetime);
            
        var response = new AuthorizationResult(encodedJwt, UserDtoConverter.ToDto(user));

        return response;
    }
}