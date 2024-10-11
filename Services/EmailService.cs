using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using FoodCart_Hexaware.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message, bool isHtml = false)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(
            _configuration["SmtpSettings:SenderName"],
            _configuration["SmtpSettings:SenderEmail"]
        ));
        emailMessage.To.Add(new MailboxAddress(string.Empty, toEmail));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder();

        // Set the body format based on isHtml flag
        if (isHtml)
        {
            bodyBuilder.HtmlBody = message; // Set HTML body
        }
        else
        {
            bodyBuilder.TextBody = message; // Fallback to plain text
        }

        emailMessage.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            try
            {
                // Connect to the SMTP server
                await client.ConnectAsync(
                    _configuration["SmtpSettings:Host"],
                    int.Parse(_configuration["SmtpSettings:Port"]),
                    bool.Parse(_configuration["SmtpSettings:EnableSSL"])
                );

                // Authenticate
                await client.AuthenticateAsync(
                    _configuration["SmtpSettings:Username"],
                    _configuration["SmtpSettings:Password"]
                );

                // Send the email
                await client.SendAsync(emailMessage);
            }
            finally
            {
                // Disconnect
                await client.DisconnectAsync(true);
            }
        }
    }
}
