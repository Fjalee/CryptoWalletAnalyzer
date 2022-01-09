using System.Collections.Generic;
using System.Threading.Tasks;
using WebScraper.Parsers;

namespace WebScraper.WebScrapers
{
    public class EtherscanDexScraper : IDexScraper
    {
        private readonly ITableParser _parser;
        private readonly IWebScraper _webScraper;

        public EtherscanDexScraper(ITableParser parser, IWebScraper webScraper)
        {
            _parser = parser;
            _webScraper = webScraper;
        }

        public async Task<List<DexRow>> ScrapeTable(string url)
        {
            var page = await _webScraper.GetPage(url);
            State.CurrentScrapingPageHtml = page;

            var table = _parser.GetTable(page);

            var allRows = new List<DexRow>();
            foreach (var row in table.Children)
            {
                var newRows = _parser.ParseRow(row);
                allRows.Add((DexRow)newRows); //fix, should convert like that
            }

            return allRows;
        }
    }
}
