using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "workout")]
public class WorkoutDto
{
    [DataMember(Name = "id")]
    public Guid Id { get; set; }

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
    
    public WorkoutDto (Guid id, string name, string description,Guid idTrainer,  TimeSpan duration, int level)
    {
        Id = id;
        Name = name;
        IdTrainer = idTrainer;
        Description = description;
        Duration = duration;
        Level = level;
    }
}