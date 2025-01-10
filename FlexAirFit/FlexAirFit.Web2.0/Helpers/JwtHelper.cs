using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlexAirFit.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace FlexAirFit.Web2._0.Helpers;

public class JwtHelper
{
    public static ClaimsIdentity CreateClaimsIdentity(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimNamesHelper.IdClaimName, user.Id.ToString()),
            new Claim(ClaimNamesHelper.RoleClaimName, user.Role.ToString())
        };
    
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
    
        return claimsIdentity;
    }

    public static SymmetricSecurityKey GetSymmetricSecurityKey(string secretKey) => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    public static string CreateJwtToken(IEnumerable<Claim> claims, string issuer, string audience, string secretKey, int lifetime)
    {
        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(lifetime)),
            signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256));
    
        var encodedJwt = new  JwtSecurityTokenHandler().WriteToken(jwt);
    
        return encodedJwt;
    }
}