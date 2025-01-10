using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "createMembership")]
public class CreateMembershipRequestDto
{
    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    [DataMember(Name = "duration")]
    public TimeSpan Duration { get; set; }

    [DataMember(Name = "price")]
    public int Price { get; set; }

    [DataMember(Name = "freezing")]
    public int Freezing { get; set; }
    
    public CreateMembershipRequestDto(string name, TimeSpan duration, int price, int freezing)
    {
        Name = name;
        Duration = duration;
        Price = price;
        Freezing = freezing;
    }
}