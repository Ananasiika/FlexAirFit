﻿using FlexAirFit.Core.Models;
using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.Commands.ClientProductCommands;

public class BuyClientProductCommand : Command
{
    public override string? Description()
    {
        return "Купить товар";
    }

    public override async Task Execute(Context context)
    {
        Console.Write("Введите id товара: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid productId))
        {
            Console.WriteLine("Ошибка: Введенное значение имеет некорректный формат для id товара");
            return;
        }
        Client client = await context.ClientService.GetClientByIdUser(context.CurrentUser.Id);
        ClientProduct clientProduct = new(client.Id, productId);
        int cost;
        try
        {
            cost = await context.ClientProductService.AddClientProductAndReturnCost(clientProduct, false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        Console.WriteLine($"К оплате {cost} рублей");
    }
}