using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto.User;

[DataContract(Name = "login")]
public class LoginDto
{
    [DataMember(Name = "email")]
    public string Email { get; set; }
    
    [DataMember(Name = "password")]
    public string Password { get; set; }

    public LoginDto(string email,
        string password)
    {
        Email = email;
        Password = password;
    }
}