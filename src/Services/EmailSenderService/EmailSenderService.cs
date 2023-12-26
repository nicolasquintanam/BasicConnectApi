using BasicConnectApi.Models;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MimeKit;

namespace BasicConnectApi.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailConfiguration _emailConfiguration;

    public EmailSenderService(EmailConfiguration emailConfiguration)
    {
        _emailConfiguration = emailConfiguration;
    }

    public async Task SendEmail(string email, string subject, string body)
    {
        using MimeMessage emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfiguration.Name, _emailConfiguration.From));
        emailMessage.To.Add(new MailboxAddress(email, email));
        emailMessage.Subject = subject;
        BodyBuilder emailBodyBuilder = new() { TextBody = body };
        emailMessage.Body = emailBodyBuilder.ToMessageBody();
        using SmtpClient mailClient = new();
        try
        {
            await mailClient.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
            await mailClient.AuthenticateAsync(_emailConfiguration.Name, _emailConfiguration.Password);
            await mailClient.SendAsync(emailMessage);
        }
        catch
        {

            throw;
        }
        finally
        {
            await mailClient.DisconnectAsync(true);
            mailClient.Dispose();
        }
    }
}