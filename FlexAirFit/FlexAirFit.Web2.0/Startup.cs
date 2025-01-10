using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Services;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Repositories;
using FlexAirFit.Web2._0.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.OpenApi.Models;

namespace FlexAirFit.Web2._0;

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
        
        services.AddControllers().AddNewtonsoftJson();
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
        
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IClientProductService, ClientProductService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IBonusService, BonusService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IMembershipService, MembershipService>();
        
        services.AddCors(o => o.AddPolicy("policy", builder =>
        {
            builder.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition");
        }));
        
        services.Configure<JwtConfiguration>(Configuration.GetSection("Auth:JwtConfiguration"));

        services.AddSwaggerGenNewtonsoftSupport();
       
        services.AddFlexAirFitSwaggerGen();
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/build";
            //configuration.RootPath = "NewApp/my-app/build";
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "FlexAirFit v1"));
    
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSpaStaticFiles();
        
        app.UseRouting();
        app.UseCors("policy");

        app.UseMiddleware<AuthJwtMiddleware>();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.UseSpa(spa =>
        {
            //spa.Options.SourcePath = "NewApp/my-app";
            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                spa.UseReactDevelopmentServer(npmScript: "start");
            }
        });
    }
}
