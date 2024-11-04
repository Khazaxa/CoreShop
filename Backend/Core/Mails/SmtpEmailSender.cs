using Core.Configuration;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Security;

namespace Core.Mails;

public class SmtpEmailSender(IAppConfiguration configuration) : IEmailSender
{
    public async Task SendEmailAsync(string to, string subject, string htmlContent)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(configuration!.Smtp.FromName, configuration.Smtp.FromEmail));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlContent };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(configuration.Smtp.Host, configuration.Smtp.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(configuration.Smtp.UserName, configuration.Smtp.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}