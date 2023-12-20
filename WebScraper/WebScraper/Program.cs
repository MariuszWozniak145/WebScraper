using Microsoft.Extensions.Configuration;
using WebScraper.Services;
using WebScraper.Services.Interfaces;

var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

IWebScraper OlxScraper = new OlxWebScraper("Scraping offers for plots of land near Nowy Sącz",
                                    "https://www.olx.pl/nowy-sacz/q-dzia%C5%82ka/?page=1&search%5Bdist%5D=15&search%5Bfilter_float_price%3Afrom%5D=30000&search%5Bfilter_float_price%3Ato%5D=300000&search%5Border%5D=created_at%3Adesc");

List<IWebScraper> scrapers = new() { OlxScraper };
IEmailService emailService = new EmailService(configuration);

ApplicationManager appManager = new ApplicationManager(configuration, scrapers, emailService);
appManager.Run();
