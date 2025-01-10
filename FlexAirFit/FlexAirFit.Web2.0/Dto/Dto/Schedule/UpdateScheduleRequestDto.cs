using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "updateSchedule")]
public class UpdateScheduleRequestDto
{
    [DataMember(Name = "idWorkout")]
    public Guid IdWorkout { get; set; }
    
    [DataMember(Name = "dateAndTime")]
    public DateTime DateAndTime { get; set; }
    
    [DataMember(Name = "idClient")]
    public Guid? IdClient { get; set; }
    
    public UpdateScheduleRequestDto (Guid idWorkout, DateTime dateAndTime, Guid? idClient)
    {
        IdWorkout = idWorkout;
        DateAndTime = dateAndTime;
        IdClient = idClient;
    }
}