using Autofac;

namespace Domain.Users;

public class UsersModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
    
        builder.RegisterType<Repositories.UserRepository>().AsImplementedInterfaces();
        builder.RegisterType<Services.UserService>().AsImplementedInterfaces();
    }
}