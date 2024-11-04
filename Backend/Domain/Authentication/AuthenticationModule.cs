using Autofac;

namespace Domain.Authentication;

public class AuthenticationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        
        builder.RegisterType<Services.AuthenticationService>().AsImplementedInterfaces();
        builder.RegisterType<Services.UserContextProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<Services.SystemUserContext>().As<Services.ISystemUserContext>().SingleInstance();
    }
}