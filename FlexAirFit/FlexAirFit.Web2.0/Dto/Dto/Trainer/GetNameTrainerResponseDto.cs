using System.Runtime.Serialization;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "nameTrainer")]
public class GetNameTrainerResponseDto
{
    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    public GetNameTrainerResponseDto(string name)
    {
        Name = name;
    }
}