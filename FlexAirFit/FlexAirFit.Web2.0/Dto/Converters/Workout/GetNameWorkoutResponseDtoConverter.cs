using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class GetNameWorkoutResponseDtoConverter
{
    public static GetNameWorkoutResponseDto? ToDto(string name)
    {
        return new GetNameWorkoutResponseDto(name);
    }
}