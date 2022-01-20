using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            var config = serviceProvider.GetService<IOptions<AppSettingsOptions>>().Value;

            var url = @"https://etherscan.io/dextracker_txns?q=0x6b3595068778dd592e39a122f4f5a5cf09c90fe2&ps=100";
            var sleepTimeMs = config.Blockchains.Etherscan.SleepTimeBetweenScrapesInMs;
            var appendPeriodInMs = config.Output.AppendPeriodInSeconds;

            await dexCollector.Start(url, sleepTimeMs, appendPeriodInMs);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var config = SetupConfiguration();

            services
                .AddAutoMapper(typeof(Program))
                .Configure<AppSettingsOptions>(
                    config.GetSection("AppSettings"))
                .Configure<ApiOptions>(
                    config.GetSection("AppSettings").GetSection("Blockchains").GetSection("Etherscan").GetSection("Api"))
                .Configure<OutputOptions>(
                    config.GetSection("AppSettings").GetSection("Output"));

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
                .AddJsonFile($"apikeys.json", false)
                .AddJsonFile($"appsettings.json", false)
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();
        }
    }
}
