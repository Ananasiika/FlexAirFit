using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class GetNameTrainerResponseDtoConverter
{
    public static GetNameTrainerResponseDto? ToDto(string name)
    {
        return new GetNameTrainerResponseDto(name);
    }
}