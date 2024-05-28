using System.ComponentModel.DataAnnotations;

namespace FlexAirFit.Web.Models;

public class AdminModel : UserModel
{
    [Required(ErrorMessage = "Пожалуйста, введите имя администратора")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 100 символов")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Пожалуйста, выберите пол")]
    public string Gender { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите дату рождения")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }
}
