using Core.Configuration.Config;
using Microsoft.Extensions.Configuration;

namespace Core.Configuration;

public class AppConfiguration(IConfiguration configuration) : IAppConfiguration
{
    public AdminConfig? Admin { get; } = configuration
        .GetSection("App:Admin")
        .Get<AdminConfig>();
}