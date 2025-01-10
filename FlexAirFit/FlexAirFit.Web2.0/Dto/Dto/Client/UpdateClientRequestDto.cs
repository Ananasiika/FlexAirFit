using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "updateClient")]
public class UpdateClientRequestDto
{
    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    [DataMember(Name = "gender")]
    [EnumDataType(typeof(GenderType))]
    public string Gender { get; set; }
    
    [DataMember(Name = "dateOfBirth")]
    public DateTime DateOfBirth { get; set; }
    
    public UpdateClientRequestDto(string name,
        string gender,
        DateTime dateOfBirth)
    {
        Name = name;
        Gender = gender;
        DateOfBirth = dateOfBirth;
    }
}