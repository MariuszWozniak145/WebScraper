using HtmlAgilityPack;
using WebScraper.Models;
using WebScraper.Services.Interfaces;

namespace WebScraper.Services;

public class OlxWebScraper : IWebScraper
{
    private readonly HtmlWeb _web;
    public string Url { get; init; }
    public string Description { get; init; }

    public OlxWebScraper(string description, string url)
    {
        Description = description;
        Url = url;
        _web = new HtmlWeb();
    }
    public List<Offer> GetOffers()
    {
        var document = _web.Load(Url);
        var elements = document.DocumentNode.SelectNodes("//a[@class='css-rc5s2u']");
        var links = GetLinks(elements);
        return links.Select(l => CreateOffer(l)).ToList();
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
        string title = document.DocumentNode.SelectSingleNode("//h4[@class='css-77x51t']")?.InnerText;
        string price = document.DocumentNode.SelectSingleNode("//h3[@class='css-93ez2t']")?.InnerText;
        string description = document.DocumentNode.SelectSingleNode("//div[@class='css-1t507yq er34gjf0']")?.InnerText;
        return new Offer(id, link, title, price, description);
    }
}
