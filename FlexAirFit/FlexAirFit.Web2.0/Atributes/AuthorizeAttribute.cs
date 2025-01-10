using FlexAirFit.Core.Enums;
using FlexAirFit.Web2._0.Dto.Dto.Auth;
using FlexAirFit.Web2._0.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FlexAirFit.Web2._0.Atributes;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private List<UserRole> Roles { get;}

    public AuthorizeAttribute(params UserRole[] roles)
    {
        if (roles == null)
            Roles = new List<UserRole> { UserRole.Guest, UserRole.Client, UserRole.Admin, UserRole.Trainer };
        else
            Roles = roles.ToList();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var roleClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimNamesHelper.RoleClaimName);

        if (roleClaim == null)
        {
            context.Result = new JsonResult(new ErrorResponse("Пользователь не авторизован."))
            {
                StatusCode = 401
            };
        }
        else if (!Roles.Select(r => r.ToString()).Contains(roleClaim.Value))
        {
            context.Result = new JsonResult(new ErrorResponse("У пользователя нет прав."))
            {
                StatusCode = 403
            };
        }
    }
}