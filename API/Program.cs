using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Configuration;
using Core.Exceptions.Middleware;
using Domain;
using Domain.Users.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();
        ConfigureDependencyInjection(builder);
        
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreShop API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
        
        var env = builder.Environment;
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        var jwtKey = builder.Configuration["App:Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
            throw new ArgumentNullException(nameof(jwtKey), "JWT Key is not configured.");

        var key = Encoding.UTF8.GetBytes(jwtKey);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["App:Jwt:Issuer"],
                ValidAudience = builder.Configuration["App:Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddControllers();
        var app = builder.Build();
        
        app.UseMiddleware<ExceptionMiddleware>();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreShop API v1");
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        using (var scope = app.Services.CreateScope())
        {
            DomainModule.MigrateDatabase(scope);
            var appConfiguration = scope.ServiceProvider.GetRequiredService<IAppConfiguration>();
            SeedInitialData(scope, appConfiguration);
            DomainModule.MigrateDatabase(scope);
        }

        app.Run();
    }
    
    private static void ConfigureDependencyInjection(WebApplicationBuilder appBuilder)
    {
        appBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        appBuilder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterModule(new DomainModule(appBuilder.Configuration));
            containerBuilder.RegisterInstance(new AppConfiguration(appBuilder.Configuration))
                .As<IAppConfiguration>().SingleInstance();
        });
    }

    private static void SeedInitialData(IServiceScope scope, IAppConfiguration configuration)
    {
        var admin = configuration.Admin;
        var services = scope.ServiceProvider;
        var userService = services.GetRequiredService<IUserService>();
        
        userService.CreateInitialUserAsync(
            admin.Email,
            admin.Password,
            admin.Name,
            admin.Surname,
            admin.AreaCode,
            admin.Phone,
            CancellationToken.None).GetAwaiter().GetResult();
    }
}