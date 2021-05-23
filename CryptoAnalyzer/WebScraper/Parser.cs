using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebScraper
{
    public class Parser
    {
        private const string _domainUrl = "https://bscscan.com"; //fix temp put into config file
        private const string _unknownTokenImg = "/images/main/empty-token.png";

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

        public async Task ParseTxnRow(IElement row)
        {
            try
            {
                if (9 != row.Children.Length ||
                    "has-tag text-truncate" != row.Children[1].Children[0].ClassName
                    )
                {
                    new Exception(); //fix temp. Warn about exception and stop program
                }

                var pathCryptoInfo = ((IHtmlAnchorElement)row.Children[8].Children[0]).PathName;
                var pageCryptoInfo = await new TempName().GetIHtmlDoc(_domainUrl + pathCryptoInfo);
                var cryptoInfo = ParseCryptoInfo(pageCryptoInfo);

                var Known = cryptoInfo.Known;
                var Token = cryptoInfo.Token;
                var TxnHash = row.Children[1].Children[0].TextContent;
                var Value = row.Children[7].TextContent;
            }
            catch
            {
                new Exception(); //fix temp. Warn about exception and stop program
            }
        }

        private CryptoInfo ParseCryptoInfo(IHtmlDocument page)
        {
            var token = new CryptoInfo();

            var tokenHtmlEl = page.All
                .Where(x => x.LocalName == "span" && x.ClassName == "text-secondary small")
                .Where(x => x.ParentElement.ClassName == "media-body")
                .Where(x => x.ParentElement.TextContent.Contains("Token"));

            token.Token = tokenHtmlEl.First().TextContent;
            token.Known = ParseCryptoImgSrc(tokenHtmlEl.First()) != _unknownTokenImg;

            if (tokenHtmlEl.Count() != 1)
            {
                new Exception(); //fix temp. Warn about exception and stop program
            }

            return token;
        }

        private string ParseCryptoImgSrc(IElement tokenHtmlEl)
        {
            var imageOuterHtml = tokenHtmlEl.ParentElement.ParentElement.Children
                .Where(x => x.LocalName == "img")
                .Where(x => x.ClassName == "u-sm-avatar mr-2")
                .Select(x => x.OuterHtml);


            var srcStart = imageOuterHtml.First().IndexOf("src=\"") + "src=\"".Length;
            var srcEnd = imageOuterHtml.First().Substring(srcStart).IndexOf("\"");


            if (imageOuterHtml.Count() != 1)
            {
                new Exception(); //fix temp. Warn about exception and stop program
            }

            return imageOuterHtml.First().Substring(srcStart, srcEnd);
        }
    }
}