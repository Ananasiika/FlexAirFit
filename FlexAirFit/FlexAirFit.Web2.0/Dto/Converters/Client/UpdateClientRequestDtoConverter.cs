using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Dto.Dto;

namespace FlexAirFit.Web2._0.Dto.Converters;

public class UpdateClientRequestDtoConverter
{
    public static Client? ToCore(UpdateClientRequestDto dto, Client? client)
    {
        if (client is null)
        {
            throw new ClientNotFoundException(client.Id);
            return null;
        }

        return new Client(client.Id, dto.Name, dto.Gender, dto.DateOfBirth, client.IdMembership, client.MembershipEnd, client.RemainFreezing, client.FreezingIntervals);
    }
}