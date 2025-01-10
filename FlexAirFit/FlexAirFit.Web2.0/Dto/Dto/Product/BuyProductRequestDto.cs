using System.Runtime.Serialization;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "buyProduct")]
public class BuyProductRequestDto
{
    [DataMember(Name = "usesBonuses")]
    public bool UsesBonuses { get; set; }

    [DataMember(Name = "userId")]
    public Guid UserId { get; set; }
    
    public BuyProductRequestDto(bool usesBonuses, Guid userId)
    {
        UsesBonuses = usesBonuses;
        UserId = userId;
    }
}