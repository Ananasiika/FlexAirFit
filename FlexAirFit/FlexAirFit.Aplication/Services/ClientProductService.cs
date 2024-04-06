using System.Transactions;
using FlexAirFit.Core.Models;
using FlexAirFit.Core.Enums;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Microsoft.Extensions.Configuration;
using FlexAirFit.Application.Utils;

namespace FlexAirFit.Application.Services;
public class ClientProductService : IClientProductService
{
    private readonly IClientProductRepository _clientProductRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBonusRepository _bonusRepository;

    public ClientProductService(IClientProductRepository clientProductRepository,
                               IProductRepository productRepository,
                               IBonusRepository bonusRepository)
    {
        _clientProductRepository = clientProductRepository;
        _productRepository = productRepository;
        _bonusRepository = bonusRepository;
    }
    
    private async Task<int> CalcCostClientProduct(ClientProduct clientProduct, bool writeOff)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json") 
            .Build();
        
        var product = await _productRepository.GetProductByIdAsync(clientProduct.IdProduct);
        var totalCost = product.Price;
        int availableBonusCount = await _bonusRepository.GetCountBonusByIdClientAsync(clientProduct.IdClient); // количество бонусов у клиента

        if (writeOff)
        {
            int maxBonusCount = (int)(totalCost * int.Parse(configuration["BonusRedemptionLimitPercentage"]) * 0.01); // максимальное количество бонусов, которые можно списать
            int bonusPayment = Math.Min(availableBonusCount, maxBonusCount);
    
            totalCost -= bonusPayment;
            availableBonusCount -= bonusPayment;
        }

        availableBonusCount += (int)(totalCost * int.Parse(configuration["EarnedBonusPercentage"]) * 0.01);
        if (product.Type == ProductType.PersonalWorkout)
            availableBonusCount += int.Parse(configuration["CountBonusesPersonal"]);

        await _bonusRepository.UpdateCountBonusByIdClientAsync(clientProduct.IdClient, availableBonusCount);

        return totalCost;
    }

    public async Task<int> AddClientProductAndReturnCost(ClientProduct clientProduct, bool writeOff)
    {
        using (var clientProductTransaction = new TransactionScope())
        {
            try
            {
                await _clientProductRepository.AddClientProductAsync(clientProduct);
                int totalCost = await CalcCostClientProduct(clientProduct, writeOff);

                clientProductTransaction.Complete();

                return totalCost;
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, откатываем транзакцию
                clientProductTransaction.Dispose();
                throw new ClientProductException("Error while processing ClientProduct", ex);
            }
        }
        
    }

}
