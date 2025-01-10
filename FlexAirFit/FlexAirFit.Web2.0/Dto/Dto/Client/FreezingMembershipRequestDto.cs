using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "freezingMembership")]
public class FreezingMembershipRequestDto
{
    [DataMember(Name = "startDate")]
    public DateTime StartDate { get; set; }
    
    [DataMember(Name = "duration")]
    [Range(7, 90)]
    public int Duration { get; set; }
    
    public FreezingMembershipRequestDto(DateTime startDate, int duration)
    {
        StartDate = startDate;
        Duration = duration;
    }
}