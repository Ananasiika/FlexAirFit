using Microsoft.EntityFrameworkCore;
using FlexAirFit.Database.Models;

namespace FlexAirFit.Database.Context;

public class FlexAirFitDbContext : DbContext
{
    public DbSet<UserDbModel> Users { get; set; }
    public DbSet<AdminDbModel> Admins { get; set; }
    public DbSet<BonusDbModel> Bonuses { get; set; }
    public DbSet<ClientDbModel> Clients { get; set; }
    public DbSet<MembershipDbModel> Memberships { get; set; }
    public DbSet<ProductDbModel> Products { get; set; }
    public DbSet<ScheduleDbModel> Schedules { get; set; }
    public DbSet<TrainerDbModel> Trainers { get; set; }
    public DbSet<WorkoutDbModel> Workouts { get; set; }
    
    public DbSet<ClientProductDbModel> ClientProducts { get; set; }
    
    public FlexAirFitDbContext(DbContextOptions options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientDbModel>()
            .HasMany(c => c.Products)
            .WithMany(p => p.Clients)
            .UsingEntity<ClientProductDbModel>(
                p => p.HasOne(e => e.Product).WithMany(e => e.ClientsProducts),
                c => c.HasOne(e => e.Client).WithMany(e => e.ClientsProducts));

        base.OnModelCreating(modelBuilder);
    }

}
