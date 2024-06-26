﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FlexAirFit.Database.Context;
using Microsoft.EntityFrameworkCore;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Database.Repositories;
using FlexAirFit.Application.Services;
using FlexAirFit.TechUI.AdminMenu;
using FlexAirFit.TechUI.BaseMenu;
using FlexAirFit.TechUI.ClientMenu;
using FlexAirFit.TechUI.TrainerMenu;
using FlexAitFit.TechUI.GuestMenu;
using Serilog;

namespace FlexAirFit.TechUI;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

        var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
        {
            services.AddDbContext<FlexAirFitDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("default"));
            });

            var menus = new List<Menu>
            {
                new GuestMenuBuilder().BuildMenu(),
                new ClientMenuBuilder().BuildMenu(),
                new AdminMenuBuilder().BuildMenu(),
                new TrainerMenuBuilder().BuildMenu()
            };

            services.AddSingleton(config);
            services.AddSingleton(menus);

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

            services.AddSingleton<Startup>();
        });
        
        try
        {
            var host = builder.Build();
            await using var context = host.Services.GetRequiredService<FlexAirFitDbContext>();

            await context.Database.MigrateAsync();

            var serviceScope = host.Services.CreateAsyncScope();

            var services = serviceScope.ServiceProvider;
            Console.WriteLine("К базе данных подключились, запускаем приложение)");
            Log.Information("Connected to the database, starting the application");
            var techUiApp = services.GetRequiredService<Startup>();
            await techUiApp.Run();
        }
        catch (Exception ex)
        { 
            Log.Fatal("Error connecting to the database: {Message}", ex.Message);
            Console.WriteLine("[!] " + ex.Message);
        }
    }
}
