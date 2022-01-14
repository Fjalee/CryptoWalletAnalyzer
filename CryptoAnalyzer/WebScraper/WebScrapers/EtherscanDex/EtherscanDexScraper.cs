using System.Collections.Generic;
using System.Threading.Tasks;
using WebScraper.Parsers;

namespace WebScraper.WebScrapers
{
    public class EtherscanDexScraper : IDexScraper
    {
        private readonly IDexTableParser _dexParser;
        private readonly IWebScraper _webScraper;
        private string _currentPageUrl;
        private int _currentPageNumber = 1;

        public EtherscanDexScraper(IDexTableParser parser, IWebScraper webScraper)
        {
            _dexParser = parser;
            _webScraper = webScraper;
        }

        public void Initialize(string url)
        {
            _currentPageUrl = url;
        }

        public async Task<List<DexRow>> ScrapeCurrentPageTable()
        {
            var page = await _webScraper.GetPage(_currentPageUrl);
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

        public void GoToNextPage()
        {
            _currentPageNumber++;
            _currentPageUrl += $"&p={_currentPageNumber}";
        }
    }
}
