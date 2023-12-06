using WebScraper.Models;

namespace WebScraper.Services.Interfaces;

public interface IWebScraper
{
    public string Url { get; init; }
    public string Description { get; init; }
    public List<Offer> GetOffers();
}
