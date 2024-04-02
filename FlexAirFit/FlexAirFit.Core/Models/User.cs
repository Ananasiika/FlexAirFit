using FlexAirFit.Core.Enums;

namespace FlexAirFit.Core.Models;

public class User
{
    public Guid Id { get; set; }
    public UserRole Role { get; set; }
    public string Email { get; set; }
    public string PasswordHashed { get; set; }
    
    public User(Guid id,
        UserRole role,
        string email,
        string passwordHashed)
    {
        Id = id;
        Role = role;
        Email = email;
        PasswordHashed = passwordHashed;
    }
}