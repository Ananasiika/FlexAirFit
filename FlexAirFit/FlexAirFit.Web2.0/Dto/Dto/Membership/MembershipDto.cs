using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "membership")]
public class MembershipDto
{
    [DataMember(Name = "id")]
    public Guid Id { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    [DataMember(Name = "duration")]
    public TimeSpan Duration { get; set; }

    [DataMember(Name = "price")]
    public int Price { get; set; }

    [DataMember(Name = "freezing")]
    public int Freezing { get; set; }

    public MembershipDto(Guid id, string name, TimeSpan duration, int price, int freezing)
    {
        Id = id;
        Name = name;
        Price = price;
        Duration = duration;
        Freezing = freezing;
    }
}