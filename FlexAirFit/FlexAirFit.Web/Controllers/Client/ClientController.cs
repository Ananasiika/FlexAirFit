using FlexAirFit.Core.Models;
using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlexAirFit.Web.Controllers;

public class ClientController : Controller
{
    private readonly Serilog.ILogger _logger = Log.ForContext<ClientController>();
    private readonly Context _context;

    public ClientController(ILogger<ClientController> logger, Context context)
    {
        _context = context;
    }
    public IActionResult ClientDashboard()
    {
        return View();
    }

    public async Task<IActionResult> Information()
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);
        
        if (userId != null && userId != Guid.Empty)
        {
            Client client = await _context.ClientService.GetClientById(userId);

            var clientModel = new ClientModel
            {
                Id = client.Id,
                Name = client.Name,
                DateOfBirth = client.DateOfBirth,
                Gender = client.Gender,
                IdMembership = client.IdMembership,
                MembershipEnd = client.MembershipEnd,
                RemainFreezing = client.RemainFreezing,
                FreezingIntervals = client.FreezingIntervals,
                NameMembership = _context.MembershipService.GetMembershipById(client.IdMembership).Result.Name,
                CountBonuses = _context.BonusService.GetCountBonusByIdClient(client.Id).Result
            };
            
            return View(clientModel);
        }
        return View();
    }

    public IActionResult EditInformation()
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);
        
        if (userId != null && userId != Guid.Empty)
        {
            var client = _context.ClientService.GetClientById(userId).Result;

            var clientModel = new ClientModel
            {
                Id = client.Id,
                Name = client.Name,
                DateOfBirth = client.DateOfBirth,
                Gender = client.Gender,
                IdMembership = client.IdMembership,
                MembershipEnd = client.MembershipEnd,
                RemainFreezing = client.RemainFreezing,
                FreezingIntervals = client.FreezingIntervals == null ? null : client.FreezingIntervals,
                NameMembership = _context.MembershipService.GetMembershipById(client.IdMembership).Result.Name,
                CountBonuses = _context.BonusService.GetCountBonusByIdClient(client.Id).Result
            };
            
            return View(clientModel);
        }
        return View();
    }

    [HttpPost]
    public IActionResult SaveChanges(ClientModel clientModel)
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);

        if (userId != null && userId != Guid.Empty)
        {
            var client = _context.ClientService.GetClientById(userId).Result;
            
            client.Name = clientModel.Name;
            client.Gender = clientModel.Gender;
            client.DateOfBirth = clientModel.DateOfBirth.ToUniversalTime();
            _context.ClientService.UpdateClient(client);
        }
        return RedirectToAction("Information", "Client");
    }

    public IActionResult FreezeMembershipView()
    {
        return View("FreezeMembership");
    }

    public async Task<IActionResult> FreezeSubscription(FreezingModel model)
    {
        var userId = Guid.Parse(Request.Cookies["UserId"]);

        if (userId != null && userId != Guid.Empty)
        {
            try
            {
                await _context.ClientService.FreezeMembership(userId, model.FreezeStart.ToDateTime(TimeOnly.MinValue), model.FreezeDuration);
                return RedirectToAction("Information", "Client");
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
        return RedirectToAction("Information", "Client");
    }

    public async Task<IActionResult> ViewClient(int page = 1)
    {
        _logger.Information("Executing ViewClient action with page {Page}", page);

        var clients = await _context.ClientService.GetClients(10, 10 * (page - 1));

        var clientsModels = clients.Select(t => new ClientModel
        {
            Id = t.Id,
            Name = t.Name,
            Gender = t.Gender,
            DateOfBirth = t.DateOfBirth,
            NameMembership = _context.MembershipService.GetMembershipById(t.IdMembership).Result.Name,
            MembershipEnd = t.MembershipEnd,      
            FreezingIntervals = t.FreezingIntervals == null ? null : t.FreezingIntervals,
            PageCurrent = page
        });

        return View(clientsModels);
    }
}