using Microsoft.EntityFrameworkCore;

namespace FlexAirFit.Database.Context;

public class InMemoryDbContextFactory() : IDbContextFactory
{
    private readonly string _dbName = $"MewingPadTestDb_{Guid.NewGuid()}";

    public FlexAirFitDbContext GetDbContext()
    {
        var builder = new DbContextOptionsBuilder<FlexAirFitDbContext>();
        builder.UseInMemoryDatabase(_dbName);

        return new(builder.Options);
    }
}