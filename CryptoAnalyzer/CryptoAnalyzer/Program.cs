using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebScraper;

namespace CryptoAnalyzer
{
    public class Program
    {
        static async Task Main()
        {
            var nmOfOutputAppends = 0;
            var appendPeriodInMs = int.Parse(ConfigurationManager.AppSettings.Get("OUTPUT_APPEND_PERIOD_IN_SECONDS")) * 1000;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var allTransactions = new List<Transaction>();

            while (true)
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

                if ((nmOfOutputAppends + 1) * appendPeriodInMs < stopwatch.ElapsedMilliseconds)
                {
                    nmOfOutputAppends++;
                    try
                    {
                        new CsvOutput().CreateFile(ConfigurationManager.AppSettings.Get("OUTPUT_PATH"), "1", allTransactions); //temp fix 1
                    }
                    catch
                    {
                        //temp add exception handling
                    }
                }
            }


            stopwatch.Stop();





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
