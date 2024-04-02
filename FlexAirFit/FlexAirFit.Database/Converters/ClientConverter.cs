using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Models.Converters
{
    public static class ClientConverter
    {
        [return: NotNullIfNotNull(nameof(model))]
        public static Client? DbToCoreModel(ClientDbModel? model)
        {
            return model is not null
                ? new Client(
                    id: model.Id,
                    idUser: model.IdUser,
                    name: model.Name,
                    gender: model.Gender,
                    dateOfBirth: model.DateOfBirth,
                    idMembership: model.IdMembership,
                    membershipEnd: model.MembershipEnd,
                    remainFreezing: model.RemainFreezing,
                    isFreezing: model.IsFreezing,
                    freezingIntervals: model.FreezingIntervals?.Select(fi => new Tuple<DateOnly, DateOnly>(fi.Item1, fi.Item2)).ToList())
                : default;
        }

        [return: NotNullIfNotNull(nameof(model))]
        public static ClientDbModel? CoreToDbModel(Client? model)
        {
            return model is not null
                ? new ClientDbModel(
                    id: model.Id,
                    idUser: model.IdUser,
                    name: model.Name,
                    gender: model.Gender,
                    dateOfBirth: model.DateOfBirth,
                    idMembership: model.IdMembership,
                    membershipEnd: model.MembershipEnd,
                    remainFreezing: model.RemainFreezing,
                    isFreezing: model.IsFreezing,
                    freezingIntervals: model.FreezingIntervals?.Select(fi => new Tuple<DateOnly, DateOnly>(fi.Item1, fi.Item2)).ToList())
                : default;
        }
    }
}