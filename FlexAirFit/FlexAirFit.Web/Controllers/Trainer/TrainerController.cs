using FlexAirFit.Core.Models;
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
            try
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
            catch (Exception e)
            {
                Response.Cookies.Append("errorType", e.GetType().Name);
                return RedirectToAction("Error", "Shared");
            }
        }
        return View();
    }

    public IActionResult EditInformation()
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);
        
        if (userId != null && userId != Guid.Empty)
        {
            try
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
            catch (Exception e)
            {
                Response.Cookies.Append("errorType", e.GetType().Name);
                return RedirectToAction("Error", "Shared");
            }
        }
        return View();
    }
    
    [HttpPost]
    public IActionResult SaveChanges(TrainerModel trainerModel)
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);

        if (userId != null && userId != Guid.Empty)
        {
            try
            {
                var trainer = _context.TrainerService.GetTrainerById(userId).Result;

                trainer.Name = trainerModel.Name;
                trainer.Gender = trainerModel.Gender;
                _context.TrainerService.UpdateTrainer(trainer);
            }
            catch (Exception e)
            {
                Response.Cookies.Append("errorType", e.GetType().Name);
                return RedirectToAction("Error", "Shared");
            }
        }
        return RedirectToAction("Information", "Trainer");
    }

    public async Task<IActionResult> DeleteTrainer(Guid trainerId)
    {
        try
        {
            await _context.TrainerService.DeleteTrainer(trainerId);
            return RedirectToAction("ViewTrainer");
        }
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }

    public IActionResult UpdateTrainer(Guid trainerId)
    {
        try
        {
            var trainer = _context.TrainerService.GetTrainerById(trainerId).Result;

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
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> EditTrainer(TrainerModel trainerModel)
    {
        try
        {
            Console.WriteLine(trainerModel.Gender);
            var trainer = new Trainer(trainerModel.Id, trainerModel.Name, trainerModel.Gender, trainerModel.Specialization, trainerModel.Experience, trainerModel.Rating);
            await _context.TrainerService.UpdateTrainer(trainer);

            return RedirectToAction("ViewTrainer");
        }
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }
}
