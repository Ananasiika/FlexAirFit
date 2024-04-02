using System.Diagnostics.CodeAnalysis;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Models.Converters;

public static class AdminConverter
{
    [return: NotNullIfNotNull(nameof(model))]
    public static Admin? DbToCoreModel(AdminDbModel? model)
    {
        return model is not null
            ? new Admin(
                id: model.Id,
                idUser: model.IdUser,
                name: model.Name,
                dateOfBirth: model.DateOfBirth,
                gender: model.Gender)
            : default;
    }

    [return: NotNullIfNotNull(nameof(model))]
    public static AdminDbModel? CoreToDbModel(Admin? model)
    {
        return model is not null
            ? new AdminDbModel(
                id: model.Id,
                idUser: model.IdUser,
                name: model.Name,
                dateOfBirth: model.DateOfBirth,
                gender: model.Gender)
            : default;
    }
}
