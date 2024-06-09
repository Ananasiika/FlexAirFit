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
            Response.Cookies.Append("errorType", ex.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }

        return RedirectToAction("ViewMembership");
    }

    public async Task<IActionResult> DeleteMembership(Guid membershipId)
    {
        try
        {
            await _context.MembershipService.DeleteMembership(membershipId);
            return RedirectToAction("ViewMembership");
        }
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }

    public IActionResult UpdateMembership(Guid membershipId)
    {
        try
        {
            Membership membership = _context.MembershipService.GetMembershipById(membershipId).Result;
            var model = new MembershipModel
            {
                Id = membership.Id,
                Name = membership.Name,
                Duration = membership.Duration,
                Price = membership.Price,
                Freezing = membership.Freezing
            };
            return View(model);
        }
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> EditMembership(MembershipModel membershipModel)
    {
        try
        {
            var membership = new Membership(membershipModel.Id, membershipModel.Name, TimeSpan.FromDays(membershipModel.DurationInDays), membershipModel.Price, membershipModel.Freezing);
            await _context.MembershipService.UpdateMembership(membership);
            return RedirectToAction("ViewMembership");
        }
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }
}
