using WebScraper;

namespace CryptoAnalyzer
{
    public class TokenOutputDto
    {
        public string Token { get; set; }
        public bool Known { get; set; }
        public double TotalValue { get; set; }
        public int TotalTxns { get; set; }
        public double TotalInaccurateValue { get; set; }
        public int TotalInaccurateTxns { get; set; }

        public TokenOutputDto(string token, bool known, TokenValueInfo valInfo)
        {
            Token = token;
            Known = known;
            AppendValue(valInfo);
        }

        public void AppendValue(TokenValueInfo newVal)
        {
            if (newVal.Inaccurate)
            {
                TotalInaccurateValue += newVal.Value;
                TotalInaccurateTxns++;
            }
            else
            {
                TotalValue += newVal.Value;
                TotalTxns++;
            }
        }
    }
}
