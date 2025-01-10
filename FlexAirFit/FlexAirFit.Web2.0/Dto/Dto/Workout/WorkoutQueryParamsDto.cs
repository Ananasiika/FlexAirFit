using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "workoutQueryParams")]
public class WorkoutQueryParamsDto
{
    [DataMember(Name = "nameWorkout")] 
    public string? NameWorkout { get; set; }
    
    [DataMember(Name = "nameTrainer")] 
    public string? NameTrainer { get; set; }
    
    [DataMember(Name = "minDuration")] 
    public TimeSpan? MinDuration { get; set; }
    
    [DataMember(Name = "maxDuration")] 
    public TimeSpan? MaxDuration { get; set; }
    
    [DataMember(Name = "minLevel")] 
    [Range(1, 5)]
    public int? MinLevel { get; set; }
    
    [DataMember(Name = "maxLevel")] 
    [Range(1, 5)]
    public int? MaxLevel { get; set; }
    
    public WorkoutQueryParamsDto() { }
    
    public WorkoutQueryParamsDto(string? nameWorkout, string? nameTrainer, TimeSpan? minDuration, TimeSpan? maxDuration, int? minLevel, int? maxLevel)
    {
        NameWorkout = nameWorkout;
        NameTrainer = nameTrainer;
        MinDuration = minDuration;
        MaxDuration = maxDuration;
        MinLevel = minLevel;
        MaxLevel = maxLevel;
    }
}