using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class CreateProductRequestDtoConverter
{
    public static Product? ToCore(CreateProductRequestDto dto)
    {
        try
        {
            return new Product(Guid.NewGuid(), dto.Type, dto.Name, dto.Price);
        }
        catch (Exception e)
        {
            throw new CreateProductRequestException();
        }
    }
}