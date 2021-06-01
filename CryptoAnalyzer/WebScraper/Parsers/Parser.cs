using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace WebScraper
{
    public abstract class Parser
    {
        protected readonly string _unknownTokenImg;

        public Parser(string unknownTokenImg)
        {
            _unknownTokenImg = unknownTokenImg;
        }

        abstract public IElement ParseTxnTable(IHtmlDocument page);

        abstract public Transaction ParseTxnRow(IElement row);

        abstract protected string ParseImgSrc(IElement row);

        abstract protected string ParseToken(IElement row);

        abstract protected string ParseTxnHash(IElement row);

        abstract protected TokenValueInfo ParseValue(IElement row);
    }
}
