using System.Configuration;
using System.Threading.Tasks;
using WebScraper;

namespace CryptoAnalyzer
{
    public class Program
    {
        static async Task Main()
        {
            var temp = await new WebScrapers(
                ConfigurationManager.AppSettings.Get("DOMAIN_NAME_1"),
                ConfigurationManager.AppSettings.Get("PATH_1"),
                ConfigurationManager.AppSettings.Get("UNKNOWN_CRYPTO_1")).ScrapePage();
        }
    }
}
