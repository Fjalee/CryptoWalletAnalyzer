using WebScraper.WebScrapers;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Diagnostics;

namespace WebScraper.Parsers
{
    public class EtherscanDexParser : ITableParser
    {
        private readonly IParserCommon _parserCommon;

        public EtherscanDexParser(IParserCommon parserCommon)
        {
            _parserCommon = parserCommon;
        }

        public IElement GetTable(IHtmlDocument page)
        {
            var current = page.Body.Children[0];
            try
            {
                _parserCommon.StepIfMatches(ref current, "wrapper", current.ClassName, current.Children[1]);
                _parserCommon.StepIfMatches(ref current, "main", current.LocalName, current.Children[5]);
                _parserCommon.StepIfMatches(ref current, "container space-bottom-2", current.ClassName, current.Children[0]);
                _parserCommon.StepIfMatches(ref current, "card", current.ClassName, current.Children[0]);
                _parserCommon.StepIfMatches(ref current, "card-body", current.ClassName, current.Children[1]);
                _parserCommon.StepIfMatches(ref current, "table-responsive mb-2 mb-md-0", current.ClassName, current.Children[0]);
                _parserCommon.StepIfMatches(ref current, "table table-hover", current.ClassName, current.Children[1]);
                _parserCommon.StepIfMatches(ref current, "tbody", current.LocalName, current);
            }
            catch
            {
                State.ExitAndLog(new StackTrace());
            }

            return current;
        }

        public IRow ParseRow(IElement rowHtml) //fix doesnt work
        {
            var row = new DexRow();

            try
            {
                if (9 != rowHtml.Children.Length ||
                    "hash-tag text-truncate" != rowHtml.Children[1].Children[0].ClassName)
                {
                    State.ExitAndLog(new StackTrace());
                }

                /* row.TxnDate = 
                row.For = */
            }
            catch
            {
                State.ExitAndLog(new StackTrace());
            }

            return row;
        }
        /*
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
        */
    }
}
