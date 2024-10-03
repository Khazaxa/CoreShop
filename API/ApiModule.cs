using API.Services;
using Autofac;

namespace API;

public class ApiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<HttpUserContext>().AsSelf();
    }
}