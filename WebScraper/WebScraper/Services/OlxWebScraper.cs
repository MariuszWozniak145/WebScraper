using HtmlAgilityPack;
using WebScraper.Models;
using WebScraper.Services.Interfaces;

namespace WebScraper.Services;

public class OlxWebScraper : IWebScraper
{
    private readonly HtmlWeb _web;
    public string Url { get; set; }
    public string Description { get; init; }

    public OlxWebScraper(string description, string initUrl)
    {
        Description = description;
        Url = initUrl;
        _web = new HtmlWeb();
    }

    public List<Offer> GetOffers()
    {
        var offersFromToday = new List<Offer>();
        var currPage = 0;
        while (true)
        {
            currPage++;
            var document = _web.Load(Url);
            var elements = document.DocumentNode.SelectNodes("//a[@class='css-rc5s2u']");
            var links = GetLinks(elements);
            var offers = links.Select(l => CreateOffer(l)).Distinct().ToList();
            var filteredOffers = offers.Where(o => o.Date.Contains("Dzisiaj") && !offersFromToday.Any(existingOffer => existingOffer.Id == o.Id)).ToList();
            if (!filteredOffers.Any()) break;
            else Url = Url.Replace($"page={currPage}", $"page={currPage + 1}");
            offersFromToday.AddRange(filteredOffers);
        }
        return offersFromToday;
    }

    private List<string> GetLinks(HtmlNodeCollection elements)
    {
        return elements.Select(e => e.GetAttributeValue("href", null))
                        .Where(v => v != null && !v.StartsWith("http"))
                        .Select(l => $"https://www.olx.pl/{l}")
                        .ToList();
    }

    private Offer CreateOffer(string link)
    {
        Thread.Sleep(50);
        var document = _web.Load(link);
        string id = document.DocumentNode.SelectSingleNode("//span[@class='css-12hdxwj er34gjf0']")?.InnerText.Split(" ")[1];
        string date = document.DocumentNode.SelectSingleNode("//span[@data-cy='ad-posted-at']")?.InnerText;
        string title = document.DocumentNode.SelectSingleNode("//h4[@class='css-77x51t']")?.InnerText;
        string price = document.DocumentNode.SelectSingleNode("//h3[@class='css-93ez2t']")?.InnerText;
        string description = document.DocumentNode.SelectSingleNode("//div[@class='css-1t507yq er34gjf0']")?.InnerText;
        return new Offer(id, link, date, title, price, description);
    }
}
