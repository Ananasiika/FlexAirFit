using System.Diagnostics.CodeAnalysis;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Converters;

public static class ProductConverter
{
    [return: NotNullIfNotNull(nameof(model))]
    public static Product? DbToCoreModel(ProductDbModel? model)
    {
        return model is not null
            ? new Product(
                id: model.Id,
                type: model.Type,
                name: model.Name,
                price: model.Price)
            : default;
    }

    [return: NotNullIfNotNull(nameof(model))]
    public static ProductDbModel? CoreToDbModel(Product? model)
    {
        return model is not null
            ? new ProductDbModel(
                id: model.Id,
                type: model.Type,
                name: model.Name,
                price: model.Price)
            : default;
    }
}
