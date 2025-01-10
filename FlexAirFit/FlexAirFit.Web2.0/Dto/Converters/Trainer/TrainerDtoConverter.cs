using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public static class TrainerDtoConverter
{
    public static TrainerDto? ToDto(this Trainer? trainer)
    {
        return trainer is null ? null : new TrainerDto(trainer.Id, trainer.Name, trainer.Gender, trainer.Specialization, trainer.Experience, trainer.Rating);
    }
    
    public static Trainer? ToCore(this TrainerDto? trainer)
    {
        return trainer is null ? null : new Trainer(trainer.Id, trainer.Name, trainer.Gender, trainer.Specialization, trainer.Experience, trainer.Rating);
    }
}