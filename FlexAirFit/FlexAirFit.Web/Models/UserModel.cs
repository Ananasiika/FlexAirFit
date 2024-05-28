using System.ComponentModel.DataAnnotations;

namespace FlexAirFit.Web.Models;

public class UserModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Введите адрес электронной почты")]
    [EmailAddress(ErrorMessage = "Некорректный формат электронной почты")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(100, ErrorMessage = "Пароль должен содержать от {2} до {1} символов", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string PasswordVerify { get; set; }

    [Required(ErrorMessage = "Введите номер роли")]
    [Range(1, 3, ErrorMessage = "Допустимые значения: 1 - клиент, 2 - админ, 3 - тренер")]
    public int RoleNumber { get; set; }
}
