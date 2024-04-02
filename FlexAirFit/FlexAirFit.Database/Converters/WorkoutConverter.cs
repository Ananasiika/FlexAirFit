using System.Diagnostics.CodeAnalysis;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Converters;

public static class WorkoutConverter
{
    [return: NotNullIfNotNull(nameof(model))]
    public static Workout? DbToCoreModel(WorkoutDbModel? model)
    {
        return model is not null
            ? new Workout(
                id: model.Id,
                name: model.Name,
                description: model.Description,
                idTrainer: model.IdTrainer,
                duration: model.Duration,
                level: model.Level)
            : default;
    }

    [return: NotNullIfNotNull(nameof(model))]
    public static WorkoutDbModel? CoreToDbModel(Workout? model)
    {
        return model is not null
            ? new WorkoutDbModel(
                id: model.Id,
                name: model.Name,
                description: model.Description,
                idTrainer: model.IdTrainer,
                duration: model.Duration,
                level: model.Level)
            : default;
    }
}
