using System.ComponentModel.DataAnnotations;

namespace FlexAirFit.Web.Models;

public class TrainerModel : UserModel
{
    [Required(ErrorMessage = "Пожалуйста, введите имя тренера")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 100 символов")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Пожалуйста, выберите пол")]
    
    public string Gender { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите специализацию тренера")]
    public string Specialization { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите опыт работы тренера в годах")]
    [Range(0, 60, ErrorMessage = "Опыт работы должен быть в диапазоне от 0 до 60 лет")]
    public int Experience { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите рейтинг тренера")]
    [Range(1, 5, ErrorMessage = "Рейтинг должен быть от 1 до 5")]
    public int Rating { get; set; }
    
    public int PageCurrent { get; set; }
}
