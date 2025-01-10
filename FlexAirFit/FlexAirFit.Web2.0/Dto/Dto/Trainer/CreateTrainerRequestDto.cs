using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "createWorkout")]
public class CreateTrainerRequestDto
{
    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    [DataMember(Name = "gender")]
    [EnumDataType(typeof(GenderType))]
    public string Gender { get; set; }
    
    [DataMember(Name = "specialization")]
    public string Specialization { get; set; }
    
    [DataMember(Name = "experience")]
    public int Experience { get; set; }

    [DataMember(Name = "rating")]
    public int Rating { get; set; }
    
    public CreateTrainerRequestDto(string name,
        string gender,
        string specialization,
        int experience,
        int rating)
    {
        Name = name;
        Gender = gender;
        Specialization = specialization;
        Experience = experience;
        Rating = rating;
    }
}