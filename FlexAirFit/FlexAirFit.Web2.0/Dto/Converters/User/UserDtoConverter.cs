using FlexAirFit.Web2._0.Dto.Dto.User;

namespace FlexAirFit.Web2._0.Dto.Converters.User;

public class UserDtoConverter
{
    public static UserDto ToDto(Core.Models.User user)
    {
        return new UserDto(user.Id, user.Email, user.PasswordHashed, user.Role);
    }
    
    public static Core.Models.User ToCore(UserDto user)
    {
        return new Core.Models.User(user.Id, user.Role, user.Email, user.Password);
    }
}