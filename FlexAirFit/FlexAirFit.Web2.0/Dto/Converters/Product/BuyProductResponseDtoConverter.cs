using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public static class BuyProductResponseDtoConverter
{
    public static BuyProductResponseDto? ToDto(int cost)
    {
        return new BuyProductResponseDto(cost);
    }   
}