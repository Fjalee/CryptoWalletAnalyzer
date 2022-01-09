using System;
using WebScraper.Parsers;
using WebScraper.WebScrapers.EtherscanDex;

namespace CryptoAnalyzer
{
    public class DexOutputDto : IRow
    {
        public DateTime TxnDate { get; set; }
        public DexAction Action { get; set; }
    }
}
