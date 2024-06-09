using System.ComponentModel.DataAnnotations;

namespace FlexAirFit.Web.Models;
public class ClientModel : UserModel
{
    [Required(ErrorMessage = "Пожалуйста, введите имя клиента")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 100 символов")]
    [Display(Name = "Имя")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Пожалуйста, выберите пол")]
    [Display(Name = "Пол")]
    public string Gender { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите дату рождения")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Дата рождения")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Пожалуйста, выберите тип членства")]
    public Guid IdMembership { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите дату окончания членства")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Дата окончания абонемента")]
    public DateTime MembershipEnd { get; set; }

    [Display(Name = "Осталось заморозки")]
    public int? RemainFreezing { get; set; }

    public bool IsMembershipActive { get; set; }
    [Display(Name = "Интервалы заморозки")]
    public DateTime?[][]? FreezingIntervals { get; set; }

    [Display(Name = "Название абонемента")]
    public string NameMembership { get; set; }
    [Display(Name = "Количество бонусов")]
    public int CountBonuses { get; set; }
    public bool ActiveMembership { get; set; }
    public int PageCurrent { get; set; }
}
