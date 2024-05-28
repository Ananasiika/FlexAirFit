using System.ComponentModel.DataAnnotations;

namespace FlexAirFit.Web.Models;
public class ClientModel : UserModel
{
    [Required(ErrorMessage = "Пожалуйста, введите имя клиента")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 100 символов")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Пожалуйста, выберите пол")]
    public string Gender { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите дату рождения")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Пожалуйста, выберите тип членства")]
    public Guid IdMembership { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите дату окончания членства")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime MembershipEnd { get; set; }

    public int? RemainFreezing { get; set; }

    public bool IsMembershipActive { get; set; }
    public DateTime?[][]? FreezingIntervals { get; set; }
    
    public string NameMembership { get; set; }
    public int CountBonuses { get; set; }
    public int PageCurrent { get; set; }
}
