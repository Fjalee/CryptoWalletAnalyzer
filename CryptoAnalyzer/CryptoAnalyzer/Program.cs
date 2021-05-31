using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using WebScraper;

namespace CryptoAnalyzer
{
    public class Program
    {
        static async Task Main()
        {
            var allTransactions = new List<Transaction>();

            for (var i = 0; i < 100; i++) //fix temp loop 10 times
            {
                var pageTransactions = await new BscscanWebScraper(
                    ConfigurationManager.AppSettings.Get("DOMAIN_NAME_BSCSCAN"),
                    ConfigurationManager.AppSettings.Get("PATH_BSCSCAN"),
                    new BscscanParser(
                        ConfigurationManager.AppSettings.Get("UNKNOWN_CRYPTO_BSCSCAN")
                        )
                    ).ScrapePage();

                allTransactions.AddRange(pageTransactions);

                Thread.Sleep(1000);
            }

        }
    }
}
