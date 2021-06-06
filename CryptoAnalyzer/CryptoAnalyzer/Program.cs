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
            var allNewTransactions = new List<Transaction>();
            var output = new List<TokenOutputDto>();


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
                    try
                    {
                        foreach (var item in allNewTransactions)
                        {
                            if (item.ValueInfo.Inaccurate)
                            {
                                var x = 0;
                            }
                        }

                        new CsvOutput().CreateFile(ConfigurationManager.AppSettings.Get("OUTPUT_PATH"), nmOfOutputAppends.ToString(), output); //temp fix 1
                        allNewTransactions.Clear();
                    }
                    catch
                    {
                        //temp add exception handling
                    }
                    finally
                    {
                        nmOfOutputAppends++;
                    }
                }
            }


            stopwatch.Stop();





            //var tooBig = new List<Transaction>();
            //foreach (var trans in allNewTransactions)
            //{
            //    if (trans.ValueInfo.Inaccurate)
            //    {
            //        tooBig.Add(trans);
            //    }
            //}
        }
    }
}
