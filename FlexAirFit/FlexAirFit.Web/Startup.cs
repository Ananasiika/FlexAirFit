using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.Services;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FlexAirFit.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<FlexAirFitDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("Default")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IBonusRepository, BonusRepository>();
        services.AddScoped<IClientProductRepository, ClientProductRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();

        services.AddScoped<Context>();

        services.AddScoped<UserService>();
        services.AddScoped<AdminService>();
        services.AddScoped<BonusService>();
        services.AddScoped<ClientProductService>();
        services.AddScoped<ClientService>();
        services.AddScoped<MembershipService>();
        services.AddScoped<ProductService>();
        services.AddScoped<ScheduleService>();
        services.AddScoped<TrainerService>();
        services.AddScoped<WorkoutService>();

        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
