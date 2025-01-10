using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public static class WorkoutDtoConverter
{
    public static WorkoutDto? ToDto(this Workout? workout)
    {
        return workout is null ? null : new WorkoutDto(workout.Id, workout.Name, workout.Description, workout.IdTrainer, workout.Duration, workout.Level);
    }
    
    public static Workout? ToCore(this WorkoutDto? workout)
    {
        return workout is null ? null : new Workout(workout.Id, workout.Name, workout.Description, workout.IdTrainer, workout.Duration, workout.Level);
    }
}