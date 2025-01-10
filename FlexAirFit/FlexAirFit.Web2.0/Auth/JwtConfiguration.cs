namespace FlexAirFit.Web2._0.Auth;

public class JwtConfiguration
{
    public required string Issuer { get; init; }

    public required string Audience { get; init; }

    public required string SecretKey { get; init; }

    public required int Lifetime { get; init; }
}