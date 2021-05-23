using System.Configuration;
using System.Threading.Tasks;
using WebScraper;

namespace CryptoAnalyzer
{
    class Program
    {
        static async Task Main()
        {
            var temp = await new WebScrapers(
                ConfigurationManager.AppSettings.Get("DOMAIN_NAME_BSCSCAN"),
                ConfigurationManager.AppSettings.Get("PATH_BSCSCAN")).Scrape();
        }
    }
}
