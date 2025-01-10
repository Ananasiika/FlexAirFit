using System.Runtime.Serialization;

namespace FlexAirFit.Web2._0.Dto.Dto.Bonus;

[DataContract(Name = "bonus")]
public class BonusDto
{
    [DataMember(Name = "bonuses")]
    public int Bonuses { get; set; }

    public BonusDto(int bonuses)
    {
        Bonuses = bonuses;
    }
}