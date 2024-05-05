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
        modelBuilder.Entity<ClientProductDbModel>()
            .HasKey(cp => new { cp.Id, cp.IdClient, cp.IdProduct });

        modelBuilder.Entity<ClientProductDbModel>()
            .HasOne(cp => cp.Client)
            .WithMany(c => c.ClientsProducts)
            .HasForeignKey(cp => cp.IdClient); 

        modelBuilder.Entity<ClientProductDbModel>()
            .HasOne(cp => cp.Product)
            .WithMany(p => p.ClientsProducts)
            .HasForeignKey(cp => cp.IdProduct);

        base.OnModelCreating(modelBuilder);
    }

}
