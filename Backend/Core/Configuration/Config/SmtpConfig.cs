namespace Core.Configuration.Config;

public record SmtpConfig(
    string Host,
    int Port,
    bool UseSsl,
    string UserName,
    string Password,
    string FromEmail,
    string FromName
    );