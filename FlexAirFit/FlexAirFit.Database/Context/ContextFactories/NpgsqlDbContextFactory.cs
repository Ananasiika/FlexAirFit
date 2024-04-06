using FlexAirFit.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FlexAirFit.Database.Context;

public class NpgsqlDbContextFactory(IConfiguration config) : IDbContextFactory
{
    private readonly IConfiguration _config = config;

    public FlexAirFitDbContext GetDbContext()
    {
        var connName = _config["Database Connection"]!;

        var builder = new DbContextOptionsBuilder<FlexAirFitDbContext>();
        builder.UseNpgsql(_config.GetConnectionString(connName));

        return new(builder.Options);
    }
}