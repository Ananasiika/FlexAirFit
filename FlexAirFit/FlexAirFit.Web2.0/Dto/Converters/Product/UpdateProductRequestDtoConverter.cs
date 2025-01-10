using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class UpdateProductRequestDtoConverter
{
    public static Product? ToCore(UpdateProductRequestDto dto, Guid id)
    {
        return new Product(id, dto.Type, dto.Name, dto.Price);
    }
}