using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class BuyProductRequestDtoConverter
{
    public static ClientProduct ToCore(BuyProductRequestDto dto, Guid id)
    {
        return new ClientProduct(dto.UserId, id);
    }
}