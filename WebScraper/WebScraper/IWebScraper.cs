namespace WebScraper;

public interface IWebScraper
{
    public List<Offer> GetOffers(string url);
}
