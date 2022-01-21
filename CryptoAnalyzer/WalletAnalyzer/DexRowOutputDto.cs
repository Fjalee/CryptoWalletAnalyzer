using System;
using WebScraper.Parsers;
using WebScraper.WebScrapers.EtherscanDex;

namespace WalletAnalyzer
{
    public class DexRowOutputDto : IRow
    {
        public string TxnHash { get; set; }
        public DateTime TxnDate { get; set; }
        public DexAction Action { get; set; }
        public string BuyerHash { get; set; }
        public string SellerHash { get; set; }
    }
}
