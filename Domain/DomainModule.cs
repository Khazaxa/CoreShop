using System.Reflection;
using Autofac;
using Domain.Authentication;
using Domain.Authentication.Services;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module = Autofac.Module;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

namespace Domain;

public class DomainModule(IConfigurationRoot configuration) : Module
{
    private const string ConnectionStringName = nameof(ShopDbContext);
    
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        
        builder.RegisterInstance(configuration).As<IConfigurationRoot>();
        builder.RegisterModule<UsersModule>();
        builder.RegisterModule<AuthenticationModule>();
        
        builder.RegisterType<UserContextService>().As<IUserContextService>().InstancePerLifetimeScope();
        
        RegisterDatabaseProviders(builder);
        RegisterMediator(builder);
    }
    
    public static void MigrateDatabase(IServiceScope scope)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
        dbContext.Database.Migrate();
    }
    
    private void RegisterDatabaseProviders(ContainerBuilder builder)
    {
        builder
            .Register(_ =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
                var connectionString = configuration.GetConnectionString(ConnectionStringName);
                if (connectionString != null)
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                return new ShopDbContext(optionsBuilder.Options);
            })
            .As<DbContext>()
            .AsSelf()
            .InstancePerDependency();
    }
    
    private static void RegisterMediator(ContainerBuilder builder)
    {
        var mediatorConfiguration = MediatRConfigurationBuilder
            .Create(Assembly.GetExecutingAssembly())
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithRegistrationScope(RegistrationScope.Scoped)
            .Build();

        builder.RegisterMediatR(mediatorConfiguration);
    }
}