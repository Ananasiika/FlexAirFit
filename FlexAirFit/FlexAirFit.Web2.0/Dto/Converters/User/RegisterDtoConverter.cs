using FlexAirFit.Web2._0.Dto.Dto.User;

namespace FlexAirFit.Web2._0.Dto.Converters.User;

public class RegisterDtoConverter
{
    public static Core.Models.User ToCore(RegisterDto user)
    {
        return new Core.Models.User(Guid.NewGuid(), user.Role, user.Email, user.Password);
    }
    
    public static RegisterDto ToDto(Core.Models.User user)
    {
        return new RegisterDto(user.Email, user.PasswordHashed, user.Role);
    }
}