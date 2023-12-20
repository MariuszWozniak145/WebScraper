using Microsoft.Extensions.Configuration;
using System.Text;
using WebScraper.Models;
using WebScraper.Services.Interfaces;

namespace WebScraper.Services;

public class ApplicationManager
{
    private readonly IConfigurationRoot _configuration;
    private readonly List<IWebScraper> _webScrapers;
    private readonly IEmailService _emailService;

    public ApplicationManager(IConfigurationRoot configuration, List<IWebScraper> webScrapers, IEmailService emailService)
    {
        _configuration = configuration;
        _webScrapers = webScrapers;
        _emailService = emailService;
    }

    public void Run()
    {
        var scrapersResults = new Dictionary<string, List<Offer>>();
        foreach (var scraper in _webScrapers)
        {
            var description = scraper.Description;
            var offers = scraper.GetOffers();
            scrapersResults.Add(description, offers);
        }

        foreach (var kvp in scrapersResults)
        {
            var toEmail = _configuration["smtp:toEmail"];
            StringBuilder emailBody = CreateEmailBody(kvp);
            _emailService.SendEmail(new EmailToSend(toEmail, kvp.Key, emailBody.ToString()));
        }
    }

    private static StringBuilder CreateEmailBody(KeyValuePair<string, List<Offer>> kvp)
    {
        var emailBody = new StringBuilder();
        var date = DateOnly.FromDateTime(DateTime.Now);
        emailBody.AppendLine($"<h1>Numbers of found offers on {date}: {kvp.Value.Count}</h1>");
        foreach (var offer in kvp.Value)
        {
            emailBody.AppendLine($"<h1 style='margin-top: 50px;'>❖ {offer.Title}</h1>");
            emailBody.AppendLine($"<h3>{offer.Price}</h3>");
            emailBody.AppendLine($"<p>{offer.Description}</p>");
            emailBody.AppendLine($"<h3>{offer.Url}</h3>");
        }
        return emailBody;
    }
}
