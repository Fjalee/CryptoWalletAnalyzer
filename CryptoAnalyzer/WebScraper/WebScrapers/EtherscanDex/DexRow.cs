using System;
using WebScraper.Parsers;

namespace WebScraper.WebScrapers
{
    public class DexRow : IRow
    {
        public DateTime TxnDate { get; set; }
        public string For { get; set; }
    }
}