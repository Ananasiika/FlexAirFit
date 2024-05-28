using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Converters;

public static class ClientConverter
{
    [return: NotNullIfNotNull(nameof(model))]
    public static Client? DbToCoreModel(ClientDbModel? model)
    {
        return model is not null
            ? new Client(
                id: model.Id,
                name: model.Name,
                gender: model.Gender,
                dateOfBirth: model.DateOfBirth,
                idMembership: model.IdMembership,
                membershipEnd: model.MembershipEnd,
                remainFreezing: model.RemainFreezing,
                freezingIntervals: model.FreezingIntervals != null ? model.FreezingIntervals.RootElement
                    .EnumerateArray()
                    .Select(interval => new DateTime?[]
                    {
                        interval.TryGetProperty("start_date", out var startDate) && startDate.ValueKind != JsonValueKind.Null
                            ? DateTime.Parse(startDate.GetString())
                            : null,
                        interval.TryGetProperty("end_date", out var endDate) && endDate.ValueKind != JsonValueKind.Null
                            ? DateTime.Parse(endDate.GetString())
                            : null
                    })
                    .ToArray() : null)
            : default;
    }

    [return: NotNullIfNotNull(nameof(model))]
    public static ClientDbModel? CoreToDbModel(Client? model)
    {
        return model is not null
            ? new ClientDbModel(
                id: model.Id,
                name: model.Name,
                gender: model.Gender,
                dateOfBirth: model.DateOfBirth,
                idMembership: model.IdMembership,
                membershipEnd: model.MembershipEnd,
                remainFreezing: model.RemainFreezing,
                freezingIntervals: model.FreezingIntervals != null && model.FreezingIntervals.Length != 0 ? JsonDocument.Parse(
                    JsonSerializer.Serialize(
                        model.FreezingIntervals.Select(interval => new
                        {
                            start_date = interval[0]?.ToString("yyyy-MM-dd"),
                            end_date = interval[1]?.ToString("yyyy-MM-dd")
                        })
                    )
                ) : JsonDocument.Parse("[]"))
            : default;
    }
}
