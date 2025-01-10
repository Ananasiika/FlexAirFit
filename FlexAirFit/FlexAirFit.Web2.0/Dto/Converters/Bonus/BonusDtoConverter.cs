using FlexAirFit.Web2._0.Dto.Dto;
using FlexAirFit.Web2._0.Dto.Dto.Bonus;

namespace FlexAirFit.Web2._0.Dto.Converters.Bonus;

public static class BonusDtoConverter
{
    public static BonusDto? ToDto(int bonuses)
    {
        return new BonusDto(bonuses);
    }   
}