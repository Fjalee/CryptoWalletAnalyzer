using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace WebScraper.WebScrapers
{
    public interface IEtherscanDexParser
    {
        public IElement ParseTable(IHtmlDocument page);
    }
}