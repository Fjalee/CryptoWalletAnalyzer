using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Threading.Tasks;

namespace WebScraper
{
    public class Parser
    {
        private const string _domainUrl = "https://bscscan.com"; //fix temp put into config file

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

            //var imageSrc = pageCryptoInfo.All
            //    .Where(x => "body" == x.LocalName)
            //    .Where(x => "div" == x.LocalName && "wrapper" == x.ClassName)
            //    .Where(x => "main" == x.LocalName && "content" == x.Id)
            //    .Where(x => "div" == x.LocalName && "container py-3" == x.ClassName)
            //    .Where(x => "div" == x.LocalName && "d-lg-flex align-items-center" == x.ClassName)
            //    .Where(x => "div" == x.LocalName && "mb-3 mb-lg-0" == x.ClassName)
            //    .Where(x => "h1" == x.LocalName && "h4 media align-items-center text-dark" == x.ClassName);

            return null; //fix temp
        }
    }
}
