using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Filters;
using FlexAirFit.Core.Models;
using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using ILogger = Serilog.ILogger;

namespace FlexAirFit.Web.Controllers;

public class ScheduleController : Controller
{
    private readonly ILogger _logger = Log.ForContext<ScheduleController>();
    private readonly Context _context;

    public ScheduleController(ILogger<ScheduleController> logger, Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> ViewSchedule(int page = 1)
    {
        _logger.Information("Executing ViewSchedule action with page {Page}", page);

        var schedules = await _context.ScheduleService.GetSchedules(10, 10 * (page - 1));

        var schedulesModels = schedules.Select(s => new ScheduleModel
        {
            Id = s.Id,
            IdWorkout = s.IdWorkout,
            DateAndTime = s.DateAndTime.AddHours(3),
            IdClient = s.IdClient,
            NameClient = s.IdClient == null || s.IdClient == Guid.Empty ? null : _context.ClientService.GetClientById((Guid)s.IdClient).Result.Name,
            IdTrainer = _context.WorkoutService.GetWorkoutById(s.IdWorkout).Result.IdTrainer,
            NameTrainer = _context.TrainerService.GetTrainerNameById(_context.WorkoutService.GetWorkoutById(s.IdWorkout).Result.IdTrainer).Result,
            NameWorkout = _context.WorkoutService.GetWorkoutNameById(s.IdWorkout).Result,
            PageCurrent = page
        });
        var workoutTypes = Enum.GetValues(typeof(WorkoutType)).Cast<WorkoutType>().ToList();

        var viewModel = new ScheduleFilterModelResult
        {
            Schedules = schedulesModels,
            Filter = new ScheduleFilterModel(),
            WorkoutTypes = workoutTypes
        };

        return View("ViewSchedule", viewModel);
    }
    
    public async Task<IActionResult> ViewScheduleFiltered(
        string? NameWorkout,
        DateTime? MinDateAndTime,
        DateTime? MaxDateAndTime,
        WorkoutType? WorkoutType,
        Guid? ClientId,
        Guid? TrainerId,
        int PageNumber = 1)
    {
        _logger.Information("Executing ViewScheduleFiltered action with page {PageNumber}", PageNumber);
        
        var filter = new FilterSchedule(
            NameWorkout,
            MinDateAndTime == null ? null : ((DateTime)MinDateAndTime).ToUniversalTime(),
            MaxDateAndTime == null  ? null : ((DateTime)MaxDateAndTime).ToUniversalTime(),
            WorkoutType,
            ClientId,
            TrainerId
        );
        
        var schedules = await _context.ScheduleService.GetScheduleByFilter(filter, 10, 10 * (PageNumber - 1));

        var scheduleModels = schedules.Select(s => new ScheduleModel
        {
            Id = s.Id,
            NameWorkout = _context.WorkoutService.GetWorkoutNameById(s.IdWorkout).Result,
            DateAndTime = s.DateAndTime.AddHours(3),
            IdClient = s.IdClient,
            NameClient = s.IdClient == null || s.IdClient == Guid.Empty ? null : _context.ClientService.GetClientById((Guid) s.IdClient).Result.Name,
            IdTrainer = _context.WorkoutService.GetWorkoutById(s.IdWorkout).Result.IdTrainer,
            NameTrainer = _context.TrainerService.GetTrainerNameById(_context.WorkoutService.GetWorkoutById(s.IdWorkout).Result.IdTrainer).Result,
            PageCurrent = PageNumber
        });
        var workoutTypes = Enum.GetValues(typeof(WorkoutType)).Cast<WorkoutType>().ToList();
        
        
        var viewModel = new ScheduleFilterModelResult
        {
            Schedules = scheduleModels,
            Filter = new ScheduleFilterModel
            {
                NameWorkout = filter.NameWorkout,
                MinDateAndTime = filter.MinDateAndTime,
                MaxDateAndTime = filter.MaxDateAndTime,
                WorkoutType = filter.WorkoutType,
                ClientId = filter.ClientId,
                TrainerId = filter.TrainerId,
                PageNumber = PageNumber
            },
            WorkoutTypes = workoutTypes
        };

        return View("ViewScheduleFiltered", viewModel);
    }

    public async Task<IActionResult> ViewScheduleByCLient(int page = 1)
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);
        var userRole = Request.Cookies["UserRole"];

        FilterSchedule filter;

        if (userRole == UserRole.Client.ToString())
        {
            filter = new(null, null, null, null, userId, null);
        }
        else 
        {
            filter = new(null, null, null, null, null, userId);
        }
        
        var schedules = await _context.ScheduleService.GetScheduleByFilter(filter, 10, 10 * (page - 1));

        var schedulesModels = schedules.Select(s => new ScheduleModel
        {
            Id = s.Id,
            IdWorkout = s.IdWorkout,
            DateAndTime = s.DateAndTime.AddHours(3),
            IdClient = s.IdClient,
            NameClient = s.IdClient == null || s.IdClient == Guid.Empty ? null : _context.ClientService.GetClientById((Guid)s.IdClient).Result.Name,
            IdTrainer = _context.WorkoutService.GetWorkoutById(s.IdWorkout).Result.IdTrainer,
            NameTrainer = _context.TrainerService.GetTrainerNameById(_context.WorkoutService.GetWorkoutById(s.IdWorkout).Result.IdTrainer).Result,
            NameWorkout = _context.WorkoutService.GetWorkoutNameById(s.IdWorkout).Result,
            PageCurrent = page
        });

        return View(schedulesModels);
    }

    [HttpGet]
    public async Task<IActionResult> CreateScheduleByClient(Guid workoutId)
    {
        var workout = await _context.WorkoutService.GetWorkoutById(workoutId);

        var model = new ScheduleModel
        {
            IdWorkout = workoutId,
            NameWorkout = workout.Name,
            NameTrainer = _context.TrainerService.GetTrainerNameById(workout.IdTrainer).Result
        };

        return View("CreateSchedule", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchedule(string WorkoutName, string TrainerName, DateTime DateAndTime, Guid IdWorkout, Guid IdClient)
    {
        try
        {
            var userRole = Request.Cookies["UserRole"].ToString();
            if (userRole == "Client")
            {
                IdClient = Guid.Parse(Request.Cookies["UserId"]);
            }
            
            var schedule = new Schedule(Guid.NewGuid(), IdWorkout, DateAndTime.ToUniversalTime(), IdClient);
            await _context.ScheduleService.CreateSchedule(schedule);
            if (userRole == "Client")
            {
                return RedirectToAction("ViewScheduleByCLient");
            }
            else
            {
                return RedirectToAction("ViewSchedule");
            }
        }
        catch (Exception e)
        {
            return View("Error", e.Message);
        }
    }

    public IActionResult CreateScheduleByAdmin()
    {
        var model = new ScheduleModel();
        return View("CreateSchedule", model);
    }

    public async Task<IActionResult> DeleteSchedule(Guid scheduleId)
    {
        await _context.ScheduleService.DeleteSchedule(scheduleId);
        return RedirectToAction("ViewSchedule");
    }
}