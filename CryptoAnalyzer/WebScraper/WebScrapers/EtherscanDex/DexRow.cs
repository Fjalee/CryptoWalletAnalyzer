using System;
using WebScraper.Parsers;
using WebScraper.WebScrapers.EtherscanDex;

namespace WebScraper.WebScrapers
{
    public class DexRow : IRow
    {
        public DateTime TxnDate { get; set; }
        public DexAction Action { get; set; }
    }
}