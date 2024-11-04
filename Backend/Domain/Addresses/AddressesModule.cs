using Autofac;

namespace Domain.Addresses;

public class AddressesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
    
        builder.RegisterType<Repositories.AddressRepository>().AsImplementedInterfaces();
        builder.RegisterType<Services.AddressService>().AsImplementedInterfaces();
    }
}