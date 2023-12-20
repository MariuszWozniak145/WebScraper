using WebScraper.Models;

namespace WebScraper.Services.Interfaces;

public interface IEmailService
{
    public void SendEmail(EmailToSend email);
}
