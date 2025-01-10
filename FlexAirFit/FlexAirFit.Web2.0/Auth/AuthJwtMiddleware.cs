using System.IdentityModel.Tokens.Jwt;
using FlexAirFit.Web2._0.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FlexAirFit.Web2._0.Auth;

public class AuthJwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly ILogger<AuthJwtMiddleware> _logger;
    
    public AuthJwtMiddleware(RequestDelegate next,
        IOptions<JwtConfiguration> jwtConfiguration,
        ILogger<AuthJwtMiddleware> logger)
    {
        _next = next;
        _jwtConfiguration = jwtConfiguration.Value;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrWhiteSpace(token))
                await AttachUserToContextAsync(context, token);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in middleware {Middleware}", nameof(AuthJwtMiddleware));
        }
        await _next(context);
    }
    private Task AttachUserToContextAsync(HttpContext context, string token, CancellationToken cancellationToken = default)
    {
        var validationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = false,
                
            // строка, представляющая издателя
            ValidIssuer = _jwtConfiguration.Issuer,
                
            // будет ли валидироваться потребитель токена
            ValidateAudience = false,
                
            // установка потребителя токена
            ValidAudience = _jwtConfiguration.Audience,
                
            // будет ли валидироваться время существования
            ValidateLifetime = false,
                
            // установка ключа безопасности
            IssuerSigningKey = JwtHelper.GetSymmetricSecurityKey(_jwtConfiguration.SecretKey),

            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        try
        {
            if (tokenHandler.CanReadToken(token))
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                context.User = principal;
            }
        }
        catch (SecurityTokenException e)
        {
            _logger.LogInformation(e, "Authorization Jwt token: {Token} is invalid", token);
        }

        return Task.CompletedTask;
    }
}