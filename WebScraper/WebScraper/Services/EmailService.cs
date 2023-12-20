using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using WebScraper.Models;
using WebScraper.Services.Interfaces;

namespace WebScraper.Services;

public class EmailService : IEmailService
{
    private readonly IConfigurationRoot _configuration;

    public EmailService(IConfigurationRoot configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(EmailToSend email)
    {
        var fromEmail = _configuration["smtp:fromEmail"];
        var password = _configuration["smtp:password"];
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(MailboxAddress.Parse(fromEmail));
        mailMessage.To.Add(MailboxAddress.Parse(email.To));
        mailMessage.Subject = email.Subject;
        mailMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = email.Body
        };

        using var smtpClient = new SmtpClient();
        smtpClient.Connect("smtp.gmail.com", 587, false);
        smtpClient.Authenticate(fromEmail, password);
        smtpClient.Send(mailMessage);
        smtpClient.Disconnect(true);
    }
}
