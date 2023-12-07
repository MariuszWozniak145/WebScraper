using WebScraper.Models;

namespace WebScraper.Services.Interfaces;

public interface IEmailService
{
    public Task SendEmail(List<Offer> Offers);
}
