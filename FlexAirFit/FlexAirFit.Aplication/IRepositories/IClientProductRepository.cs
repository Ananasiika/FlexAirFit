using FlexAirFit.Core.Models;

namespace FlexAirFit.Application.IRepositories;

public interface IClientProductRepository
{
    Task AddClientProductAsync(ClientProduct clientProduct);
}