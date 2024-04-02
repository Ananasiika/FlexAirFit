using System.Diagnostics.CodeAnalysis;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Models.Converters
{
    public static class UserConverter
    {
        [return: NotNullIfNotNull(nameof(model))]
        public static User? DbToCoreModel(UserDbModel? model)
        {
            return model is not null
                ? new User(
                    id: model.Id,
                    role: model.Role,
                    email: model.Email,
                    passwordHashed: model.PasswordHashed)
                : default;
        }

        [return: NotNullIfNotNull(nameof(model))]
        public static UserDbModel? CoreToDbModel(User? model)
        {
            return model is not null
                ? new UserDbModel(
                    id: model.Id,
                    role: model.Role,
                    email: model.Email,
                    passwordHashed: model.PasswordHashed)
                : default;
        }
    }
}