using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebScraper
{
    public abstract class WebScraper
    {
        protected readonly string _domainUrl;
        protected readonly string _path;
        protected readonly Parser _parser;

        public WebScraper(string domainUrl, string path, Parser parser)
        {
            _domainUrl = domainUrl;
            _path = path;
            _parser = parser;
        }

        public async Task<List<Transaction>> ScrapePage()
        {
            var scrapedPageHtml = await new TempName().GetIHtmlDoc(_domainUrl + "/" + _path);

            var txnTable = _parser.ParseTxnTable(scrapedPageHtml);

            var allTxn = new List<Transaction>();
            foreach (var row in txnTable.Children)
            {
                allTxn.Add(_parser.ParseTxnRow(row));
            }

            return allTxn;
        }
    }
}
