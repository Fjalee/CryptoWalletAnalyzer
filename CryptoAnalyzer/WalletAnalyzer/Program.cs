using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebScraper;
using WebScraper.Parsers;
namespace WalletAnalyzer
{
    public class Program
    {
        static async Task Main()
        {
            DI.Initialize();
            var config = DI.Create<IConfiguration>();
            var dexCollector = DI.Create<IDexCollector>();

            var url = @"https://etherscan.io/dextracker_txns?q=0x6b3595068778dd592e39a122f4f5a5cf09c90fe2";
            var sleepTimeMs = Int32.Parse(config["SLEEP-TIME-IN-MILISECONDS"]);
            var appendPeriodInMs = Int32.Parse(config.GetSection("OUTPUT")["APPEND-PERIOD-IN-SECONDS"]);

            await dexCollector.Start(url, sleepTimeMs, appendPeriodInMs);
        }
    }
}
