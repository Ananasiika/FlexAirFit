using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace FlexAirFit.Web.Models;

public class ScheduleModel
{
    public const IEnumerable Workouts = null;
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Пожалуйста, выберите тренировку")]
    public Guid IdWorkout { get; set; }
    public string NameWorkout { get; set; }

    [Required(ErrorMessage = "Пожалуйста, выберите дату и время")]
    public DateTime DateAndTime { get; set; }

    public Guid? IdClient { get; set; }
    public string NameClient { get; set; }

    public Guid? IdTrainer { get; set; }
    public string NameTrainer { get; set; }
    
    public int PageCurrent { get; set; }
}