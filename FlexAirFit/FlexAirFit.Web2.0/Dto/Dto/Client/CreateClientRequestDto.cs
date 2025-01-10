using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "createClient")]
public class CreateClientRequestDto
{
    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    [DataMember(Name = "gender")]
    [EnumDataType(typeof(GenderType))]
    public string Gender { get; set; }
    
    [DataMember(Name = "dateOfBirth")]
    public DateTime DateOfBirth { get; set; }
    
    [DataMember(Name = "idMembership")]
    public Guid IdMembership { get; set; }
    
    public CreateClientRequestDto(
        string name,
        string gender,
        DateTime dateOfBirth,
        Guid idMembership)
    {
        Name = name;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        IdMembership = idMembership;
    }
}