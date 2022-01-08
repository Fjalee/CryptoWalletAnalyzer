using System.Collections.Generic;
using System.Threading.Tasks;
using WebScraper.Parsers;

namespace WebScraper.WebScrapers
{
    public class EtherscanDexScraper : IEtherscanDexScraper
    {
        private readonly ITableParser _parser;
        private readonly IWebScraper _webScraper;

        public EtherscanDexScraper(ITableParser parser, IWebScraper webScraper)
        {
            _parser = parser;
            _webScraper = webScraper;
        }

        public async Task<List<IRow>> ScrapeTable(string url)
        {
            var page = await _webScraper.GetPage(url); //_domainUrl + "/" + _path
            State.CurrentScrapingPageHtml = page;

            var table = _parser.GetTable(page);

            var allRows = new List<IRow>();
            foreach (var row in table.Children)
            {
                allRows.Add(_parser.ParseRow(row));
            }

            return allRows;
        }
    }
}
