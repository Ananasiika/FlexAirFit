using System;
using System.ComponentModel.DataAnnotations;

namespace FlexAirFit.Web.Models
{
    public class WorkoutModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название тренировки")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно содержать от 2 до 100 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите описание тренировки")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Описание должно содержать от 10 до 500 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберите тренера")]
        public Guid IdTrainer { get; set; }
        
        [Required(ErrorMessage = "Пожалуйста, выберите тренера")]
        public string NameTrainer { get; set; }

        [Required(ErrorMessage = "Пожалуйста, укажите длительность тренировки")]
        public TimeSpan Duration { get; set; }

        [Required(ErrorMessage = "Пожалуйста, укажите уровень сложности тренировки")]
        [Range(1, 5, ErrorMessage = "Уровень сложности должен быть в диапазоне от 1 до 5")]
        public int Level { get; set; }
        
        public int PageCurrent { get; set; }
    }
}