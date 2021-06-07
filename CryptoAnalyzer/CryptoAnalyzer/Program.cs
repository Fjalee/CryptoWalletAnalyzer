using System;
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
            var outputName = DateTime.Now.ToString("yyyy.MM.dd-HH;mm");
            var allNewTransactions = new List<Transaction>();
            var output = new List<TokenOutputDto>();

            stopwatch.Start();
            while (true)
            {
                var pageTransactions = await new BscscanWebScraper(
                    ConfigurationManager.AppSettings.Get("DOMAIN_NAME_BSCSCAN"),
                    ConfigurationManager.AppSettings.Get("PATH_BSCSCAN"),
                    new BscscanParser(
                        ConfigurationManager.AppSettings.Get("UNKOWN_CRYPTO_IMG_BSCSCAN")
                        )
                    ).ScrapePage();

                allNewTransactions.AddRange(pageTransactions);

                Thread.Sleep(1000);

                if ((nmOfOutputAppends + 1) * appendPeriodInMs < stopwatch.ElapsedMilliseconds)
                {
                    output = new Output().Append(output, allNewTransactions);
                    var ts = stopwatch.Elapsed;
                    var timeOutput = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

                    try
                    {
                        new CsvOutput().WriteFile(ConfigurationManager.AppSettings.Get("OUTPUT_PATH"), outputName, output, timeOutput);
                        allNewTransactions.Clear();
                        nmOfOutputAppends++;
                    }
                    catch
                    {
                        //temp add exception handling
                    }
                }
            }
        }
    }
}
