using Microsoft.OpenApi.Models;

public static class ServiceProviderExtensions
{
    public static IServiceCollection AddFlexAirFitSwaggerGen(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "FlexAirFit API",
                    Version = "v1",
                    Description = "FlexAirFit API",
                });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        /*services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "FlexAirFit API",
                    Version = "v1",
                    Description = "FlexAirFit API",
                });

            options.EnableAnnotations();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            foreach (var supProjectXmlFiles in dir.EnumerateFiles("*.xml"))
            {
                options.IncludeXmlComments(supProjectXmlFiles.FullName);
            }
        });*/

        return services;
    }
}
