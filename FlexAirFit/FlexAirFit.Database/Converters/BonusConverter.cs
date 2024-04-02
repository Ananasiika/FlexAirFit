using System.Diagnostics.CodeAnalysis;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Converters;

public static class BonusConverter
{
    [return: NotNullIfNotNull(nameof(model))]
    public static Bonus? DbToCoreModel(BonusDbModel? model)
    {
        return model is not null
            ? new Bonus(
                id: model.Id,
                idClient: model.IdClient,
                count: model.Count)
            : default;
    }

    [return: NotNullIfNotNull(nameof(model))]
    public static BonusDbModel? CoreToDbModel(Bonus? model)
    {
        return model is not null
            ? new BonusDbModel(
                id: model.Id,
                idClient: model.IdClient,
                count: model.Count)
            : default;
    }
}
