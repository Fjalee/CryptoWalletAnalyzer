using AngleSharp.Dom;

namespace WebScraper
{
    public class Transaction
    {
        public string TxnHash { get; set; }
        public TokenValueInfo ValueInfo { get; set; }
        public bool Known { get; set; }
        public string Token { get; set; }
        public IElement Element { get; set; }
        public string ImgSrc { get; set; }
    }
}
