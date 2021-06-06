namespace CryptoAnalyzer
{
    public class OutputDto
    {
        public bool Known { get; set; }
        public double TotalValue { get; set; }
        public int TotalTxns { get; set; }
        public double TotalInaccurateValue { get; set; }
        public int TotalInaccurateTxns { get; set; }
    }
}
