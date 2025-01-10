using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "updateWorkout")]
public class UpdateWorkoutRequestDto
{
    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    [DataMember(Name = "description")]
    public string Description { get; set; }
    
    [DataMember(Name = "idTrainer")]
    public Guid IdTrainer { get; set; }
    
    [DataMember(Name = "duration")]
    public TimeSpan Duration { get; set; }

    [DataMember(Name = "level")]
    [Range(1, 5)]
    public int Level { get; set; }
    
    public UpdateWorkoutRequestDto(string name, string description, Guid idTrainer, TimeSpan duration, int level)
    {
        Name = name;
        Duration = duration;
        Description = description;
        IdTrainer = idTrainer;
        Level = level;
    }
}