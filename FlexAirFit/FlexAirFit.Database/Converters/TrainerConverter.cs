using System.Diagnostics.CodeAnalysis;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Models.Converters
{
    public static class TrainerConverter
    {
        [return: NotNullIfNotNull(nameof(model))]
        public static Trainer? DbToCoreModel(TrainerDbModel? model)
        {
            return model is not null
                ? new Trainer(
                    id: model.Id,
                    idUser: model.IdUser,
                    name: model.Name,
                    gender: model.Gender,
                    specialization: model.Specialization,
                    experience: model.Experience,
                    rating: model.Rating)
                : default;
        }

        [return: NotNullIfNotNull(nameof(model))]
        public static TrainerDbModel? CoreToDbModel(Trainer? model)
        {
            return model is not null
                ? new TrainerDbModel(
                    id: model.Id,
                    idUser: model.IdUser,
                    name: model.Name,
                    gender: model.Gender,
                    specialization: model.Specialization,
                    experience: model.Experience,
                    rating: model.Rating)
                : default;
        }
    }
}