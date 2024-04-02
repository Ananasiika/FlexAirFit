using System.Diagnostics.CodeAnalysis;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Models.Converters
{
    public static class ScheduleConverter
    {
        [return: NotNullIfNotNull(nameof(model))]
        public static Schedule? DbToCoreModel(ScheduleDbModel? model)
        {
            return model is not null
                ? new Schedule(
                    id: model.Id,
                    idWorkout: model.IdWorkout,
                    dateAndTime: model.DateAndTime,
                    idClient: model.IdClient.HasValue ? model.IdClient.Value : default)
                : default;
        }

        [return: NotNullIfNotNull(nameof(model))]
        public static ScheduleDbModel? CoreToDbModel(Schedule? model)
        {
            return model is not null
                ? new ScheduleDbModel(
                    id: model.Id,
                    idWorkout: model.IdWorkout,
                    dateAndTime: model.DateAndTime,
                    idClient: model.IdClient.HasValue ? model.IdClient.Value : default)
                : default;
        }
    }
}