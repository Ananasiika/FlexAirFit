using System.ComponentModel.DataAnnotations;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web.Models;

public class ProductModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Пожалуйста, выберите тип продукта")]
    public ProductType Type { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите название продукта")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Название продукта должно содержать от 2 до 100 символов")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите цену продукта")]
    public int Price { get; set; }

    public int PageCurrent { get; set; }
}