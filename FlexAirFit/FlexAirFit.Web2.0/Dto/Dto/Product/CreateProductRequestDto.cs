using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "createProduct")]
public class CreateProductRequestDto
{
    [DataMember(Name = "type")]
    [EnumDataType(typeof(ProductType))]
    public ProductType Type { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "price")]
    public int Price { get; set; }
    
    public CreateProductRequestDto(ProductType type, string name, int price)
    {
        Type = type;
        Name = name;
        Price = price;
    }
}