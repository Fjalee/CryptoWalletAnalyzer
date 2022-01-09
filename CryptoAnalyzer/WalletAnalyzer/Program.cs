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
            var url = @"https://etherscan.io/dextracker_txns?q=0x6b3595068778dd592e39a122f4f5a5cf09c90fe2";
            await new EtherscanDexScraper(new EtherscanDexParser(new ParserCommon()), new WebScraper.WebScraper()).ScrapeTable(url);
        }
    }
}
