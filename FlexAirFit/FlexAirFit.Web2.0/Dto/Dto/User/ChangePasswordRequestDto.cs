using System.Runtime.Serialization;

namespace FlexAirFit.Web2._0.Dto.Dto.User;

[DataContract(Name = "changePassword")]
public class ChangePasswordRequestDto
{
    [DataMember(Name = "oldPassword")]
    public string OldPassword { get; set; }
    
    [DataMember(Name = "newPassword")]
    public string NewPassword { get; set; }

    public ChangePasswordRequestDto(string oldPassword,
        string newPassword)
    {
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}