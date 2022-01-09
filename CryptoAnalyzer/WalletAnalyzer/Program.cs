using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebScraper;
using WebScraper.Parsers;
using WebScraper.WebScrapers;

namespace WalletAnalyzer
{
    public class Program
    {
        static async Task Main()
        {
            var scraper = new EtherscanDexScraper(new EtherscanDexParser(new ParserCommon()), new WebScraper.WebScraper());
            var config = new ConfigurationBuilder().AddJsonFile(@"appsettings.json").Build();
            var output = new DexCsvOutput(config);

            var url = @"https://etherscan.io/dextracker_txns?q=0x6b3595068778dd592e39a122f4f5a5cf09c90fe2";
            var sleepTimeMs = Int32.Parse(config["SLEEP-TIME-IN-MILISECONDS"]);
            var appendPeriodInMs = Int32.Parse(config.GetSection("OUTPUT")["APPEND-PERIOD-IN-SECONDS"]);

            await new DexCollector(scraper, output).Start(url, sleepTimeMs, appendPeriodInMs);
        }
    }

        /*    Trace.Listeners.Add(new TextWriterTraceListener(ConfigurationManager.AppSettings.Get("LOG_PATH")));
            Trace.AutoFlush = true;
            State.ScrapeDate = DateTime.Now.ToString("yyyy_MM_dd_HHmm");

            var isLeftUnsavedData = false;
            long msWorthOfDataWriten = 0;
            var appendPeriodInMs = int.Parse(ConfigurationManager.AppSettings.Get("OUTPUT_APPEND_PERIOD_IN_SECONDS")) * 1000;
            var sleepTime = int.Parse(ConfigurationManager.AppSettings.Get("SLEEP_TIME_IN_MILISECONDS"));

            var stopwatch = new Stopwatch();
            var allNewRows = new List<IRow>();
            var totalRowsScraped = 0;
            var output = new List<TokenOutputDto>();

            stopwatch.Start();
            while (true)
            {
                var pageRows = _scraper.ScrapeTable(_url);
                allNewRows.AddRange(pageRows);
                totalRowsScraped += pageRows.Count;

                Thread.Sleep(sleepTime);

                if (msWorthOfDataWriten + appendPeriodInMs < stopwatch.ElapsedMilliseconds
                    || isLeftUnsavedData)
                {
                    TryOutput();
                }
            }
        }

        static private void TryOutput()
        {
            output = new Output().Append(output, allNewRows); // rename
            allNewRows.Clear();

            var ts = stopwatch.Elapsed;
            var timeOutput = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

            try
            {
                _myOutput.DoOutput(State.ScrapeDate, output, timeOutput, totalRowsScraped);
                //new CsvOutput().WriteFile(ConfigurationManager.AppSettings.Get("OUTPUT_PATH"), State.ScrapeDate, output, timeOutput, totalRowsScraped);
                msWorthOfDataWriten = stopwatch.ElapsedMilliseconds;

                if (isLeftUnsavedData)
                {
                    isLeftUnsavedData = false;
                    Console.WriteLine("Output file closed. Scraped data was added SUCCESSFULLY...");
                }

                Console.WriteLine("Appended data scraped in " + timeOutput);
            }
            catch
            {
                isLeftUnsavedData = true;
                Console.WriteLine("Scraped data could not be added. Please close the output file...");
            }

        }
    }*/
}
