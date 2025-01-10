using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "schedule")]
public class ScheduleDto
{
    [DataMember(Name = "id")]
    public Guid Id { get; set; }

    [DataMember(Name = "idWorkout")]
    public Guid IdWorkout { get; set; }
    
    [DataMember(Name = "dateAndTime")]
    public DateTime DateAndTime { get; set; }
    
    [DataMember(Name = "idClient")]
    public Guid? IdClient { get; set; }
    
    public ScheduleDto (Guid id, Guid idWorkout, DateTime dateAndTime, Guid? idClient)
    {
        Id = id;
        IdWorkout = idWorkout;
        DateAndTime = dateAndTime;
        IdClient = idClient;
    }
}