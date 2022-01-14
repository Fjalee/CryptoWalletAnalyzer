using System.Collections.Generic;
using System.Threading.Tasks;
using WebScraper.Parsers;

namespace WebScraper.WebScrapers
{
    public class EtherscanDexScraper : IDexScraper
    {
        private readonly IDexTableParser _dexParser;
        private readonly IWebScraper _webScraper;

        public EtherscanDexScraper(IDexTableParser parser, IWebScraper webScraper)
        {
            _dexParser = parser;
            _webScraper = webScraper;
        }

        public async Task<List<DexRow>> ScrapeTable(string url)
        {
            var page = await _webScraper.GetPage(url);
            State.CurrentScrapingPageHtml = page;

            var table = _dexParser.GetTable(page);

            var allRows = new List<DexRow>();
            foreach (var row in table.Children)
            {
                var newRows = _dexParser.ParseRow(row);
                allRows.Add(newRows);
            }

            return allRows;
        }
    }
}
