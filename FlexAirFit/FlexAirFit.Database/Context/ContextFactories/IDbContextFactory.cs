namespace FlexAirFit.Database.Context;

public interface IDbContextFactory
{
    FlexAirFitDbContext GetDbContext();
}