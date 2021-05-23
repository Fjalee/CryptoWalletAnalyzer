using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebScraper
{
    public class WebScrapers
    {
        private readonly string _domainUrl;
        private readonly string _path;

        public WebScrapers(string domainUrl, string path)
        {
            _domainUrl = domainUrl;
            _path = path;
        }

        public async Task<List<Transaction>> Scrape()
        {
            var scrapedPage = await new TempName().GetIHtmlDoc(_domainUrl + "/" + _path);

            var paraser = new Parser(_domainUrl);
            var txnTable = paraser.ParseTxnTable(scrapedPage);

            var allTxn = new List<Transaction>();
            foreach (var row in txnTable.Children)
            {
                allTxn.Add(await paraser.ParseTxnRow(row));
            }

            return allTxn;
        }
    }
}
