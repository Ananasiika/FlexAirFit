using FlexAirFit.Application.Services;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public static class ClientDtoConverter
{
    public static ClientDto? ToDto(this Client? client)
    {
        return client is null ? null : new ClientDto(client.Id, client.Name, client.Gender, client.DateOfBirth, client.IdMembership, null, client.MembershipEnd, client.RemainFreezing, client.FreezingIntervals);
    }
    
    public static Client? ToCore(this ClientDto? client)
    {
        return client is null ? null : new Client(client.Id, client.Name, client.Gender, client.DateOfBirth, client.IdMembership, client.MembershipEnd, client.RemainFreezing, client.FreezingIntervals);
    }
}