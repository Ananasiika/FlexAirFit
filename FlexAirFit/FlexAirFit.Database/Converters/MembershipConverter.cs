using System.Diagnostics.CodeAnalysis;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Models.Converters;

public static class MembershipConverter
{
    [return: NotNullIfNotNull(nameof(model))]
    public static Membership? DbToCoreModel(MembershipDbModel? model)
    {
        return model is not null
            ? new Membership(
                id: model.Id,
                name: model.Name,
                duration: model.Duration,
                price: model.Price,
                freezing: model.Freezing)
            : default;
    }

    [return: NotNullIfNotNull(nameof(model))]
    public static MembershipDbModel? CoreToDbModel(Membership? model)
    {
        return model is not null
            ? new MembershipDbModel(
                id: model.Id,
                name: model.Name,
                duration: model.Duration,
                price: model.Price,
                freezing: model.Freezing)
            : default;
    }
}
