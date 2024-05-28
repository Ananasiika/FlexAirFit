using FlexAirFit.Application.IRepositories;
using FlexAirFit.Core.Filters;
using FlexAirFit.Core.Models;
using FlexAirFit.Web;
using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

public class WorkoutController : Controller
{
    private readonly ILogger _logger = Log.ForContext<WorkoutController>();
    private readonly Context _context;

    public WorkoutController(ILogger<WorkoutController> logger, Context context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> ViewWorkout(int page = 1)
    {
        _logger.Information("Executing ViewWorkoutsFiltered action with page {page}", page);

        var workouts = await _context.WorkoutService.GetWorkouts(10, 10 * (page - 1));

        var workoutModels = workouts.Select(w => new WorkoutModel
        {
            Id = w.Id,
            Name = w.Name,
            Description = w.Description,
            IdTrainer = w.IdTrainer,
            NameTrainer = _context.TrainerService.GetTrainerNameById(w.IdTrainer).Result,
            Duration = w.Duration,
            Level = w.Level,
            PageCurrent = page
        });

        var viewModel = new WorkoutFilterModelResult
        {
            Workouts = workoutModels,
            Filter = new WorkoutFilterModel()
        };

        return View("ViewWorkout", viewModel);
    }
    
    public async Task<IActionResult> ViewWorkoutFiltered(
        string Name, 
        string NameTrainer,
        TimeSpan? MinDuration,
        TimeSpan? MaxDuration,
        int? MinLevel,
        int? MaxLevel,
        int PageNumber = 1)
    {
        _logger.Information("Executing ViewWorkoutsFiltered action with page {PageNumber}", PageNumber);

        var filter = new FilterWorkout
        (
            Name,
            NameTrainer,
            MinDuration,
            MaxDuration,
            MinLevel,
            MaxLevel
        );

        var workouts = await _context.WorkoutService.GetWorkoutByFilter(filter, 10, 10 * (PageNumber - 1));

        var workoutModels = workouts.Select(w => new WorkoutModel
        {
            Id = w.Id,
            Name = w.Name,
            Description = w.Description,
            IdTrainer = w.IdTrainer,
            NameTrainer = _context.TrainerService.GetTrainerNameById(w.IdTrainer).Result,
            Duration = w.Duration,
            Level = w.Level,
            PageCurrent = PageNumber
        });

        var viewModel = new WorkoutFilterModelResult
        {
            Workouts = workoutModels,
            Filter = new WorkoutFilterModel
            {
                Name = filter.Name,
                NameTrainer = filter.NameTrainer,
                MinDuration = filter.MinDuration,
                MaxDuration = filter.MaxDuration,
                MinLevel = filter.MinLevel,
                MaxLevel = filter.MaxLevel,
                PageNumber = PageNumber
            }
        };

        return View("ViewWorkoutFiltered", viewModel);
    }

    public IActionResult CreateWorkout()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateWorkout(WorkoutModel model)
    {
        try
        {
            var userRole = Request.Cookies["UserRole"].ToString();
            if (userRole == "Trainer")
            {
                model.IdTrainer = Guid.Parse(Request.Cookies["UserId"]);
            }
            var workout = new Workout(Guid.NewGuid(), model.Name, model.Description, model.IdTrainer, model.Duration, model.Level);
           
            await _context.WorkoutService.CreateWorkout(workout);
            return RedirectToAction("ViewWorkout");
        }
        catch (Exception ex)
        {
            return View("Error", ex.Message);
        }
    }
    
    public async Task<IActionResult> DeleteWorkout(Guid workoutId)
    {
        await _context.WorkoutService.DeleteWorkout(workoutId);
        return RedirectToAction("ViewWorkout");
    }
}
