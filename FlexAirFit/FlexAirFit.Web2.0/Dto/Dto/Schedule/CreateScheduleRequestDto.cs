using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "createSchedule")]
public class CreateScheduleRequestDto
{
    [DataMember(Name = "idWorkout")]
    public Guid IdWorkout { get; set; }
    
    [DataMember(Name = "dateAndTime")]
    public DateTime DateAndTime { get; set; }
    
    [DataMember(Name = "idClient")]
    public Guid? IdClient { get; set; }
    
    public CreateScheduleRequestDto(string name, Guid idWorkout, DateTime dateAndTime, Guid? idClient)
    {
        IdWorkout = idWorkout;
        DateAndTime = dateAndTime;
        IdClient = idClient;
    }
}