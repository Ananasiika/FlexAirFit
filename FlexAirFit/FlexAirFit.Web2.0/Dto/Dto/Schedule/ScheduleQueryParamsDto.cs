using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "scheduleQueryParams")]
public class ScheduleQueryParamsDto
{
    [DataMember(Name = "nameWorkout")] 
    public string? NameWorkout { get; set; }
    
    [DataMember(Name = "minDateAndTimeItem")] 
    public DateTime? MinDateAndTime { get; set; }
    
    [DataMember(Name = "maxDateAndTimeItem")] 
    public DateTime? MaxDateAndTime { get; set; }
    
    [DataMember(Name = "workoutType")] 
    [EnumDataType(typeof(WorkoutType))]
    public WorkoutType? WorkoutType { get; set; }
    
    [DataMember(Name = "clientId")] 
    public Guid? ClientId { get; set; }
    
    [DataMember(Name = "trainerId")] 
    public Guid? TrainerId { get; set; }
    
    public ScheduleQueryParamsDto() { }
    
    public ScheduleQueryParamsDto(string? nameWorkout,
        DateTime? minDateAndTime,
        DateTime? maxDateAndTime,
        WorkoutType? workoutType,
        Guid? clientId,
        Guid? trainerId)
    {
        NameWorkout = nameWorkout;
        MinDateAndTime = minDateAndTime;
        MaxDateAndTime = maxDateAndTime;
        WorkoutType = workoutType;
        ClientId = clientId;
        TrainerId = trainerId;
    }
}