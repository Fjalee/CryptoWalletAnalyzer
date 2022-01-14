using Microsoft.Extensions.Configuration;
using Ninject.Modules;
using WalletAnalyzer;
using WebScraper;
using WebScraper.Parsers;
using WebScraper.WebScrapers;

namespace WalletAnalyzer
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            var configuration = SetupConfiguration();

            Bind<IConfiguration>().ToMethod(ctx => SetupConfiguration()).InSingletonScope();
            Bind<IDexOutput>().To<DexCsvOutput>().InTransientScope();
            Bind<IParserCommon>().To<ParserCommon>().InTransientScope();
            Bind<IWebScraper>().To<WebScraper.WebScraper>().InTransientScope();
            Bind<IDexTableParser>().To<EtherscanDexParser>().InTransientScope();
            Bind<IDexScraper>().To<EtherscanDexScraper>().InTransientScope();
            Bind<IDexCollector>().To<DexCollector>().InTransientScope();
        }

        private IConfiguration SetupConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json")
                .Build();
        }
    }
}
