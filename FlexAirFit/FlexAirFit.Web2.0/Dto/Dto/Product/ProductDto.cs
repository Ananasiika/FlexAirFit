using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

[DataContract(Name = "product")]
public class ProductDto
{
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    
    [DataMember(Name = "type")]
    [EnumDataType(typeof(ProductType))]
    public ProductType Type { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "price")]
    public int Price { get; set; }
    
    public ProductDto(Guid id, ProductType type, string name, int price)
    {
        Id = id;
        Type = type;
        Name = name;
        Price = price;
    }
}