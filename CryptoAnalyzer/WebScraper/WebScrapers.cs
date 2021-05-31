using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebScraper
{
    public class WebScrapers
    {
        private readonly string _domainUrl;
        private readonly string _path;
        private readonly string _unknownTokenImg;

        public WebScrapers(string domainUrl, string path, string unknownTokenImg)
        {
            _domainUrl = domainUrl;
            _path = path;
            _unknownTokenImg = unknownTokenImg;
        }

        public async Task<List<Transaction>> ScrapePage()
        {
            var scrapedPage = await new TempName().GetIHtmlDoc(_domainUrl + "/" + _path);

            var paraser = new Parser(_domainUrl, _unknownTokenImg);
            var txnTable = paraser.ParseTxnTable(scrapedPage);

            var allTxn = new List<Transaction>();
            foreach (var row in txnTable.Children)
            {
                allTxn.Add(paraser.ParseTxnRow(row));
            }

            return allTxn;
        }
    }
}
