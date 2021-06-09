using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
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
            State.CurrentScrapingPage = await GetIHtmlDoc(_domainUrl + "/" + _path);

            var txnTable = _parser.ParseTxnTable(State.CurrentScrapingPage);

            var allTxn = new List<Transaction>();
            foreach (var row in txnTable.Children)
            {
                allTxn.Add(_parser.ParseTxnRow(row));
            }

            return allTxn;
        }

        public async Task<IHtmlDocument> GetIHtmlDoc(string siteUrl)
        {
            var cancellationToken = new CancellationTokenSource();
            var httpClient = new HttpClient();

            var request = await httpClient.GetAsync(siteUrl);
            cancellationToken.Token.ThrowIfCancellationRequested();

            var response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            var parser = new HtmlParser();

            cancellationToken.Dispose();
            httpClient.Dispose();

            return parser.ParseDocument(response);
        }
    }
}
