
using WebScraper.Services;

var OlxScraper = new OlxWebScraper("Scraping offers for plots of land near Nowy Sącz",
                                    "https://www.olx.pl/nowy-sacz/q-dzia%C5%82ka/?search%5Bdist%5D=15&search%5Border%5D=created_at:desc&search%5Bfilter_float_price:from%5D=10000&search%5Bfilter_float_price:to%5D=300000");

var offers = OlxScraper.GetOffers();

foreach (var offer in offers)
{
    Console.WriteLine(offer);
}