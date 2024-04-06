using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IServices;

public interface IClientProductService
{
    Task<int> AddClientProductAndReturnCost(ClientProduct clientProduct, bool writeOff);
}
