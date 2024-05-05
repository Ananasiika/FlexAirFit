using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Converters;

public static class ClientProductConverter
{
    [return: NotNullIfNotNull(nameof(model))]
    public static ClientProduct? DbToCoreModel(ClientProductDbModel? model)
    {
        return model is not null
            ? new ClientProduct(
                idClient: model.IdClient,
                idProduct: model.IdProduct)
            : default;
    }

    [return: NotNullIfNotNull(nameof(model))]
    public static ClientProductDbModel? CoreToDbModel(ClientProduct? model)
    {
        return model is not null
            ? new ClientProductDbModel(
                id: Guid.NewGuid(),
                idClient: model.IdClient,
                idProduct: model.IdProduct)
            : default;
    }
}