﻿using WebScraper.WebScrapers;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Diagnostics;
using System;
using WebScraper.WebScrapers.EtherscanDex;
using System.Threading.Tasks;

namespace WebScraper.Parsers
{
    public class EtherscanDexParser : IDexTableParser
    {
        private readonly IParserCommon _parserCommon;
        private readonly IEtherscanApiServices _etherscanApiServices;

        public EtherscanDexParser(IParserCommon parserCommon, IEtherscanApiServices etherscanApiServices)
        {
            _parserCommon = parserCommon;
            _etherscanApiServices = etherscanApiServices;
        }

        public IElement GetTable(IHtmlDocument page)
        {
            IElement current = null;
            try
            {
                current = page.Body.Children[10];
                _parserCommon.StepIfMatches(ref current, "doneloadingframe", current.Id, current.Children[2]);
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

        public bool IsNoMorePages(IHtmlDocument page)
        {
            return page.Title == "Etherscan Error Page";
        }

        public async Task<DexRow> ParseRow(IElement rowHtml)
        {
            var row = new DexRow();

            try
            {
                if (10 != rowHtml.Children.Length)
                {
                    State.ExitAndLog(new StackTrace());
                }

                row.TxnHash = ParseTransactionHash(rowHtml);

                var txnDetails = await _etherscanApiServices.GetTransactionDetailsAsync(row.TxnHash);

                row.TxnDate = ParseDate(rowHtml);
                row.Action = ParseAction(rowHtml);
                row.ToHash = txnDetails?.From ?? "";
                row.FromHash = txnDetails?.To ?? "";
            }
            catch(Exception e)
            {
                State.ExitAndLog(new StackTrace());
            }

            return row;
        }

        public string ParseTokenName(IHtmlDocument page)
        {
            IElement current;
            var result = String.Empty;
            try
            {
                current = page.Body.Children[0];
                _parserCommon.StepIfMatches(ref current, current.ClassName, "wrapper", current.Children[1]);
                _parserCommon.StepIfMatches(ref current, current.Id, "content", current.Children[8]);
                _parserCommon.StepIfMatches(ref current, current.ClassName, "container py-3", current.Children[0]);
                _parserCommon.StepIfMatches(ref current, current.ClassName, "d-lg-flex align-items-center", current.Children[0]);
                _parserCommon.StepIfMatches(ref current, current.ClassName, "mb-3 mb-lg-0", current.Children[0]);
                _parserCommon.StepIfMatches(ref current, current.ClassName, "h4 media align-items-center text-dark", current.Children[2]);
                _parserCommon.StepIfMatches(ref current, current.ClassName, "media-body", current.Children[0]);
                result = _parserCommon.GetDataIfMatches(current.TextContent, current.ClassName, "text-secondary small");
            }
            catch
            {
                State.ExitAndLog(new StackTrace());
            }

            return result;
        }

        private string ParseSoldTokenName(IElement rowHtml)
        {
            return ParseTokenName(rowHtml, 5);
        }

        private string ParseBoughtTokenName(IElement rowHtml)
        {
            return ParseTokenName(rowHtml, 6);
        }

        private string ParseTokenName(IElement rowHtml, int tokenCellIndex)
        {
            var tokenCell = rowHtml.Children[tokenCellIndex];

            _parserCommon.StepIfMatches(ref tokenCell, "text-nowrap", tokenCell.ClassName, tokenCell.Children[0]);
            return _parserCommon.GetDataIfMatches(tokenCell.InnerHtml, tokenCell.LocalName, "a");
        }

        private DateTime ParseDate(IElement rowHtml)
        {
            var dateRow = rowHtml.Children[2];
            var parsedTxnDate = _parserCommon.GetDataIfMatches(dateRow.Children[0].InnerHtml, "showDate ", dateRow.ClassName);
            return DateTime.Parse(parsedTxnDate);
        }

        private DexAction ParseAction(IElement rowHtml)
        {
            var isBuyRow = rowHtml.Children[4];
            var parsedAction = isBuyRow.Children.Length == 0 ?
                _parserCommon.GetDataIfMatches(isBuyRow.InnerHtml, "td", isBuyRow.LocalName) :
                _parserCommon.GetDataIfMatches(isBuyRow.Children[0].InnerHtml, "span", isBuyRow.Children[0].LocalName);


            var successfulParse = Enum.TryParse(parsedAction, out DexAction result);
            if (!successfulParse)
            {
                throw new InvalidOperationException($"row Action was expected to be equal to Buy or Sell");
            }
            
            return result;
        }
        
        private string ParseTransactionHash(IElement rowHtml)
        {
            var dateRow = rowHtml.Children[1].Children[0];
            return _parserCommon.GetDataIfMatches(dateRow.TextContent, "hash-tag text-truncate myFnExpandBox_searchVal", dateRow.ClassName);
        }

    }
}
