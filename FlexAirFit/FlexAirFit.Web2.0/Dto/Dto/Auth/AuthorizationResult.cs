using System.Runtime.Serialization;
using FlexAirFit.Web2._0.Dto.Dto.User;

namespace FlexAirFit.Web2._0.Dto.Dto.Auth;

[DataContract(Name = "AuthorizationResult")]
public class AuthorizationResult
{
    [DataMember(Name = "jwtToken")]
    public string JwtToken { get; set; }

    [DataMember(Name = "user")]
    public UserDto User { get; set; }

    public AuthorizationResult(string jwtToken, UserDto user)
    {
        JwtToken = jwtToken;
        this.User = user;
    }
}