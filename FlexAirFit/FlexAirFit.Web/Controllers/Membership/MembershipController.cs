using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Services;
using FlexAirFit.Core.Models;
using FlexAirFit.Web;
using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace FlexAirFit.Web.Controllers;

public class MembershipController : Controller
{
    private readonly ILogger _logger = Log.ForContext<MembershipController>();
    private readonly Context _context;

    public MembershipController(ILogger<MembershipController> logger, Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> ViewMembership(int page = 1)
    {
        _logger.Information("Executing ViewMembership action with page {Page}", page);

        var memberships = await _context.MembershipService.GetMemberships(10, 10 * (page - 1));
        
        var membershipModels = memberships.Select(m => new MembershipModel
        {
            Id = m.Id,
            Name = m.Name,
            Duration = m.Duration,
            Price = m.Price,
            Freezing = m.Freezing,
            PageCurrent = page
        });

        return View(membershipModels);
    }

    public IActionResult CreateMembership()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMembership(MembershipModel membershipModel)
    {
        try
        {
            var membership = new Membership (Guid.NewGuid(), membershipModel.Name, TimeSpan.FromDays(membershipModel.DurationInDays), membershipModel.Price, membershipModel.Freezing);
            await _context.MembershipService.CreateMembership(membership);
        }
        catch (Exception ex)
        {
            return View("Error", ex.Message);
        }

        return RedirectToAction("ViewMembership");
    }

    public async Task<IActionResult> DeleteMembership(Guid membershipId)
    {
        await _context.MembershipService.DeleteMembership(membershipId);
        return RedirectToAction("ViewMembership");
    }
}
