using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using WebScraper;

namespace CryptoAnalyzer
{
    public class Program
    {
        static async Task Main()
        {
            var allTransactions = new List<Transaction>();

            for (var i = 0; i < 10; i++) //fix temp loop 10 times
            {
                var pageTransactions = await new BscscanWebScraper(
                    ConfigurationManager.AppSettings.Get("DOMAIN_NAME_1"),
                    ConfigurationManager.AppSettings.Get("PATH_1"),
                    new BscscanParser(
                        ConfigurationManager.AppSettings.Get("UNKNOWN_CRYPTO_1")
                        )
                    ).ScrapePage();

                allTransactions.AddRange(pageTransactions);
            }

        }
    }
}
