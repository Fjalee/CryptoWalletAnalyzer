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

            for (var i = 0; i < 1; i++) //fix temp loop 10 times
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

            new CsvOutput().CreateFile(ConfigurationManager.AppSettings.Get("OUTPUT_PATH"), "1", allTransactions); //temp fix 1







            //var tooBig = new List<Transaction>();
            //foreach (var trans in allTransactions)
            //{
            //    if (trans.ValueInfo.Inaccurate)
            //    {
            //        tooBig.Add(trans);
            //    }
            //}
        }
    }
}
