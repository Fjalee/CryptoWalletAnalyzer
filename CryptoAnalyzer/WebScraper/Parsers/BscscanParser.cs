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
                //transaction.Element = row; //fix temp for debugging remove when done
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

            var imgSrc = ((IHtmlImageElement)imgElements.First()).Source;
            if (imgSrc == "" || imgSrc == null)
            {
                throw new Exception();
            }

            return imgSrc;
        }

        override protected string ParseToken(IElement row)
        {
            var token = row.Children[8].TextContent;

            if (token.Contains("\n\n\n"))
            {
                token = token.Substring(token.IndexOf("\n\n\n") + "\n\n\n".Length);
            }

            if (token == "" || token == null)
            {
                throw new Exception();
            }

            return token.Substring(1); // space at the start
        }

        override protected string ParseTxnHash(IElement row)
        {
            var txnHash = row.Children[1].Children[0].TextContent;
            if (txnHash == "" || txnHash == null)
            {
                throw new Exception();
            }

            return txnHash;
        }

        override protected TokenValueInfo ParseValue(IElement row)
        {
            try
            {
                var scrapedValString = row.Children[7].TextContent;

                if (scrapedValString == "" || scrapedValString == null)
                {
                    throw new Exception();
                }

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