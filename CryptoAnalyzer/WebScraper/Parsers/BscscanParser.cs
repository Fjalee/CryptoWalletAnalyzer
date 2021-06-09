using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Diagnostics;
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
                    State.ExitAndLog(new StackTrace());
                }

                transactionsTable = page.Body.Children[0].Children[1].Children[5].Children[0].Children[0].Children[1].Children[0].Children[1];
            }
            catch
            {
                State.ExitAndLog(new StackTrace());
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
                    State.ExitAndLog(new StackTrace());
                }

                transaction.Known = ParseImgSrc(row) != _unknownTokenImg;
                transaction.Token = ParseToken(row);
                transaction.TxnHash = ParseTxnHash(row);
                transaction.ValueInfo = ParseValue(row);
            }
            catch
            {
                State.ExitAndLog(new StackTrace());
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
                State.ExitAndLog(new StackTrace());
            }

            var imgSrc = ((IHtmlImageElement)imgElements.First()).Source;
            var baseUrlScheme = ((IHtmlImageElement)imgElements.First()).BaseUrl.Scheme + @"://";
            if (imgSrc == "" || imgSrc == null || baseUrlScheme == "" || baseUrlScheme == null)
            {
                State.ExitAndLog(new StackTrace());
            }

            if (imgSrc.IndexOf(baseUrlScheme) == 0)
            {
                imgSrc = imgSrc.Substring(baseUrlScheme.Length);
            }
            else
            {
                State.ExitAndLog(new StackTrace());
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
                State.ExitAndLog(new StackTrace());
            }

            return token.Substring(1); // space at the start
        }

        override protected string ParseTxnHash(IElement row)
        {
            var txnHash = row.Children[1].Children[0].TextContent;
            if (txnHash == "" || txnHash == null)
            {
                State.ExitAndLog(new StackTrace());
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
                    State.ExitAndLog(new StackTrace());
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
            catch
            {
                State.ExitAndLog(new StackTrace());
                return null;
            }
        }
    }
}