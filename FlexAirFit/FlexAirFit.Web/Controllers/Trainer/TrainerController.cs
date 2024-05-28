using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using ILogger = Serilog.ILogger;

namespace FlexAirFit.Web.Controllers;

public class TrainerController : Controller
{
    private readonly ILogger _logger = Log.ForContext<TrainerController>();
    private readonly Context _context;

    public TrainerController(ILogger<TrainerController> logger, Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> ViewTrainer(int page = 1)
    {
        _logger.Information("Executing ViewTrainer action with page {Page}", page);

        var trainers = await _context.TrainerService.GetTrainers(10, 10 * (page - 1));

        var trainerModels = trainers.Select(t => new TrainerModel
        {
            Id = t.Id,
            Name = t.Name,
            Gender = t.Gender,
            Specialization = t.Specialization,
            Experience = t.Experience,
            Rating = t.Rating,
            PageCurrent = page
        });

        return View(trainerModels);
    }

    public IActionResult TrainerDashboard()
    {
        return View();
    }

    public async Task<IActionResult> Information()
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);
        
        if (userId != null && userId != Guid.Empty)
        {
            var trainer = _context.TrainerService.GetTrainerById(userId).Result;

            var trainerModel = new TrainerModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Gender = trainer.Gender,
                Specialization = trainer.Specialization,
                Experience = trainer.Experience,
                Rating = trainer.Rating
            };
            
            return View(trainerModel);
        }
        return View();
    }

    public IActionResult EditInformation()
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);
        
        if (userId != null && userId != Guid.Empty)
        {
            var trainer = _context.TrainerService.GetTrainerById(userId).Result;

            var trainerModel = new TrainerModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Gender = trainer.Gender,
                Specialization = trainer.Specialization,
                Experience = trainer.Experience,
                Rating = trainer.Rating
            };
            
            return View(trainerModel);
        }
        return View();
    }
    
    [HttpPost]
    public IActionResult SaveChanges(TrainerModel trainerModel)
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);

        if (userId != null && userId != Guid.Empty)
        {
            var trainer = _context.TrainerService.GetTrainerById(userId).Result;
            
            trainer.Name = trainerModel.Name;
            trainer.Gender = trainerModel.Gender;
            _context.TrainerService.UpdateTrainer(trainer);
        }
        return RedirectToAction("Information", "Trainer");
    }
}
