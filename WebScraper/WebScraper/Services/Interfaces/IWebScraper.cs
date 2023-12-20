using WebScraper.Models;

namespace WebScraper.Services.Interfaces;

public interface IWebScraper
{
    public string Url { get; set; }
    public string Description { get; init; }
    public List<Offer> GetOffers();
}
