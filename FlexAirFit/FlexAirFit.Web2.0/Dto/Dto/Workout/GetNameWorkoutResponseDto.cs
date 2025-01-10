using System.Runtime.Serialization;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "nameWorkout")]
public class GetNameWorkoutResponseDto
{
    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    public GetNameWorkoutResponseDto(string name)
    {
        Name = name;
    }
}