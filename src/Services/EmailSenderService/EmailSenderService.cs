using BasicConnectApi.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace BasicConnectApi.Services;

public class EmailSenderService : IEmailSenderService
{
    const string APP_NAME = "BasicConnect";
    const string URL_APP = "http://localhost:5001";
    const string ENDPOINT_CONFIRM = "/api/v1/emailconfirmation/confirm";
    private readonly EmailConfiguration _emailConfiguration;
    private readonly ILogger<EmailSenderService> _logger;

    public EmailSenderService(EmailConfiguration emailConfiguration, ILogger<EmailSenderService> logger)
    {
        _emailConfiguration = emailConfiguration;
        _logger = logger;
    }

    public async Task SendToConfirmEmail(string? email, string? otp)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            return;
        string name = "Nicolas";
        string subject = $"Confirm Your Email Address for {APP_NAME}";
        string body = $"Hello {name},\n\n" +
              $"Thank you for registering with {APP_NAME}. To complete the registration process, we need you to confirm your email address.\n\n" +
              $"Please use the following OTP code to confirm your email:\n{otp}\n\n" +
              $"If you did not create an account on {APP_NAME}, you can ignore this email.\n\n" +
              $"Thanks,\nThe {APP_NAME} Team";
        await SendEmail(email, subject, body);
    }

    public async Task SendToRecoverPassword(string? email, string? otp)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            return;
        string name = "Nicolas";
        string subject = $"Password Reset Request for {APP_NAME}";
        string body = $"Hello {name},\n\n" +
              $"We received a request to reset your password for {APP_NAME}. If you made this request, please use the following OTP code to reset your password:\n{otp}\n\n" +
              $"If you did not request a password reset or didn't create an account on {APP_NAME}, you can ignore this email. Your account security is important to us.\n\n" +
              $"Thanks,\nThe {APP_NAME} Team";

        await SendEmail(email, subject, body);
    }

    private async Task SendEmail(string email, string subject, string body)
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