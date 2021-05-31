using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebScraper
{
    public class Parser
    {
        private readonly string _domainUrl;
        private readonly string _unknownTokenImg;

        public Parser(string domainUrl, string unknownTokenImg)
        {
            _domainUrl = domainUrl;
            _unknownTokenImg = unknownTokenImg;
        }

        public IElement ParseTxnTable(IHtmlDocument page)
        {
            IElement transactionsTable = null;

            try
            {
                if ("wrapper" != page.Body.Children[0].ClassName ||
                    "main" != page.Body.Children[0].Children[1].LocalName ||
                    "container space-bottom-2" != page.Body.Children[0].Children[1].Children[5].ClassName ||
                    "card" != page.Body.Children[0].Children[1].Children[5].Children[0].ClassName ||
                    "card-body" != page.Body.Children[0].Children[1].Children[5].Children[0].Children[0].ClassName ||
                    "table-responsive mb-2 mb-md-0" != page.Body.Children[0].Children[1].Children[5].Children[0].Children[0].Children[1].ClassName ||
                    "table table-hover" != page.Body.Children[0].Children[1].Children[5].Children[0].Children[0].Children[1].Children[0].ClassName ||
                    "tbody" != page.Body.Children[0].Children[1].Children[5].Children[0].Children[0].Children[1].Children[0].Children[1].LocalName
                    )
                {
                    new Exception(); //fix temp. Warn about exception and stop program
                }

                transactionsTable = page.Body.Children[0].Children[1].Children[5].Children[0].Children[0].Children[1].Children[0].Children[1];
            }
            catch
            {
                new Exception(); //fix temp. Warn about exception and stop program
            }

            return transactionsTable;
        }

        public async Task<Transaction> ParseTxnRow(IElement row)
        {
            var transaction = new Transaction();

            try
            {
                if (9 != row.Children.Length ||
                    "has-tag text-truncate" != row.Children[1].Children[0].ClassName
                    )
                {
                    new Exception(); //fix temp. Warn about exception and stop program
                }

                transaction.Known = (ParseCryptoImgSrc(row) != _unknownTokenImg);
                transaction.Token = row.Children[8].TextContent;
                transaction.TxnHash = row.Children[1].Children[0].TextContent;
                transaction.Value = Convert.ToDouble(row.Children[7].TextContent);
            }
            catch
            {
                new Exception(); //fix temp. Warn about exception and stop program
            }

            return transaction;
        }

        private string ParseCryptoImgSrc(IElement row)
        {
            var imgElements = row.Children[8].Children[0].Children
                .Where(x => x.LocalName == "noscript")
                .Where(x => x.Children[0].LocalName == "img")
                .Select(x => x.Children[0]);

            return ((IHtmlImageElement)imgElements.FirstOrDefault()).Source;
        }
    }
}