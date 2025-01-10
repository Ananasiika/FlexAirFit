using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public static class ProductDtoConverter
{
    public static ProductDto? ToDto(this Product? product)
    {
        return product is null ? null : new ProductDto(product.Id, product.Type, product.Name, product.Price);
    }   
    
    public static Product? ToCore(this ProductDto? product)
    {
        return product is null ? null : new Product(product.Id, product.Type, product.Name, product.Price);
    }
}