using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Linq;

namespace WebScraper
{
    public class BscscanParser : Parser
    {
        public BscscanParser(string unknownTokenImg) : base(unknownTokenImg) { }

        override public IElement ParseTxnTable(IHtmlDocument page)
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

        override public Transaction ParseTxnRow(IElement row)
        {
            var transaction = new Transaction();

            try
            {
                if (9 != row.Children.Length ||
                    "has-tag text-truncate" != row.Children[1].Children[0].ClassName)
                {
                    new Exception(); //fix temp. Warn about exception and stop program
                }

                transaction.Known = ParseImgSrc(row) != _unknownTokenImg;
                transaction.Token = ParseToken(row);
                transaction.TxnHash = ParseTxnHash(row);
                transaction.ValueInfo = ParseValue(row);
            }
            catch
            {
                throw new Exception(); //fix temp. Warn about exception and stop program
            }

            return transaction;
        }

        override protected string ParseImgSrc(IElement row)
        {
            var imgElements = row.Children[8].Children[0].Children
                .Where(x => x.LocalName == "noscript")
                .Where(x => x.Children[0].LocalName == "img")
                .Select(x => x.Children[0]);

            if (imgElements.Count() != 1)
            {
                throw new Exception(); //fix temp. Warn about exception and stop program
            }

            return ((IHtmlImageElement)imgElements.First()).Source;
        }

        override protected string ParseToken(IElement row)
        {
            return row.Children[8].TextContent;
        }

        override protected string ParseTxnHash(IElement row)
        {
            return row.Children[1].Children[0].TextContent;
        }

        override protected TokenValueInfo ParseValue(IElement row)
        {
            try
            {
                var scrapedValString = row.Children[7].TextContent;
                if (scrapedValString.Contains("..."))
                {
                    scrapedValString = scrapedValString.Replace("...", String.Empty);

                    if (!scrapedValString.Contains('.'))
                    {
                        return new TokenValueInfo(Convert.ToDouble(scrapedValString), true);
                    }
                }

                return new TokenValueInfo(Convert.ToDouble(scrapedValString), false);
            }
            catch (Exception)
            {
                throw; //fix temp. Warn about exception and stop program
            }
        }
    }
}