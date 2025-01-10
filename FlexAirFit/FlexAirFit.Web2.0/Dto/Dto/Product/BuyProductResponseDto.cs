using System.Runtime.Serialization;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "buyProductResponse")]
public class BuyProductResponseDto
{
    [DataMember(Name = "cost")]
    public int Cost { get; set; }
    
    public BuyProductResponseDto(int cost)
    {
        Cost = cost;
    }
}