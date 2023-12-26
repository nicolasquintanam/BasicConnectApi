using BasicConnectApi.Models;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MimeKit;

namespace BasicConnectApi.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailConfiguration _emailConfiguration;
    private readonly ILogger<EmailSenderService> _logger;

    public EmailSenderService(EmailConfiguration emailConfiguration, ILogger<EmailSenderService> logger)
    {
        _emailConfiguration = emailConfiguration;
        _logger = logger;
    }

    public async Task SendEmail(string email, string subject, string body)
    {
        using MimeMessage emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From, _emailConfiguration.From));
        emailMessage.To.Add(new MailboxAddress(email, email));
        emailMessage.Subject = subject;
        BodyBuilder emailBodyBuilder = new() { TextBody = body };
        emailMessage.Body = emailBodyBuilder.ToMessageBody();
        using SmtpClient mailClient = new();
        try
        {
            await mailClient.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
            await mailClient.AuthenticateAsync(_emailConfiguration.From, _emailConfiguration.Password);
            await mailClient.SendAsync(emailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email.");
            throw;
        }
        finally
        {
            await mailClient.DisconnectAsync(true);
            mailClient.Dispose();
        }
    }
}