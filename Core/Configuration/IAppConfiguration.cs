using Core.Configuration.Config;

namespace Core.Configuration;

public interface IAppConfiguration
{
    AdminConfig? Admin { get; }
    SmtpConfig? Smtp { get; }
}