using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using WalletAnalyzer.TemporaryTesting;
using WebScraper;
using WebScraper.Parsers;
using WebScraper.WebScrapers;

namespace WalletAnalyzer
{
    public class Program
    {
        static async Task Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var dexCollector = serviceProvider.GetService<IDexCollector>();
            var config = serviceProvider.GetService<IConfiguration>();

            var url = @"https://etherscan.io/dextracker_txns?q=0x6b3595068778dd592e39a122f4f5a5cf09c90fe2&ps=100";
            var sleepTimeMs = Int32.Parse(config["SLEEP-TIME-IN-MILISECONDS"]);
            var appendPeriodInMs = Int32.Parse(config.GetSection("OUTPUT")["APPEND-PERIOD-IN-SECONDS"]);

            await dexCollector.Start(url, sleepTimeMs, appendPeriodInMs);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IConfiguration>(SetupConfiguration())
                .AddAutoMapper(typeof(Program));

            services
                .AddTransient<IDexOutput, DexCsvOutput>()
                .AddTransient<IParserCommon, ParserCommon>()
                .AddTransient<IWebScraper, WebScraper.WebScraper>()
                //.AddTransient<IWebScraper, MockWebScraper>()
                .AddTransient<IDexTableParser, EtherscanDexParser>()
                .AddTransient<IDexScraper, EtherscanDexScraper>()
                .AddTransient<IDexCollector, DexCollector>()
                .AddTransient<IDexScraperFactory, DexScraperFactory>()
                .AddSingleton<IEtherscanApiServices, EtherscanApiServices>();
        }

        private static IConfiguration SetupConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", false)
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();
        }
    }
}
