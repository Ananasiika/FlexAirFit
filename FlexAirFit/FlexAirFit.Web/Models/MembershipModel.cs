using System;
using System.ComponentModel.DataAnnotations;

namespace FlexAirFit.Web.Models;
public class MembershipModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите название абонемента")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно содержать от 2 до 100 символов")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Пожалуйста, укажите длительность абонемента")]
    public TimeSpan Duration { get; set; }
    public int DurationInDays { get; set; }

    [Required(ErrorMessage = "Пожалуйста, укажите цену абонемента")]
    [Range(1, int.MaxValue, ErrorMessage = "Цена должна быть положительным числом")]
    public int Price { get; set; }

    [Required(ErrorMessage = "Пожалуйста, укажите количество дней для заморозки абонемента")]
    [Range(0, int.MaxValue, ErrorMessage = "Количество дней для заморозки должно быть целым положительным числом")]
    public int Freezing { get; set; }
    
    public int PageCurrent { get; set; }
}
public class FreezingModel
{
    [Required(ErrorMessage = "Пожалуйста, укажите дату начала заморозки")]
    [DataType(DataType.Date)]
    public DateOnly FreezeStart { get; set; }

    [Required(ErrorMessage = "Пожалуйста, укажите длительность заморозки")]
    [Range(1, 365, ErrorMessage = "Длительность заморозки должна быть в диапазоне от 1 до 365 дней")]
    public int FreezeDuration { get; set; }

    public Guid IdClient { get; set; }
}


