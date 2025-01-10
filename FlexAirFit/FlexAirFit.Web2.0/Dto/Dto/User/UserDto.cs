using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto.User;

[DataContract(Name = "user")]
public class UserDto
{
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    
    [DataMember(Name = "email")]
    public string Email { get; set; }
    
    [DataMember(Name = "password")]
    public string Password { get; set; }
    
    [DataMember(Name = "Role")]
    [EnumDataType(typeof(UserRole))]
    public UserRole Role { get; set; }

    public UserDto(Guid id,
        string email,
        string password,
        UserRole role)
    {
        Id = id;
        Email = email;
        Password = password;
        Role = role;
    }
}