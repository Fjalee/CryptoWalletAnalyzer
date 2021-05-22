namespace WebScraper
{
    public class Transaction
    {
        public string TxnHash { get; set; }
        public double Value { get; set; }
        public bool Known { get; set; }
        public string Token { get; set; }
    }
}
