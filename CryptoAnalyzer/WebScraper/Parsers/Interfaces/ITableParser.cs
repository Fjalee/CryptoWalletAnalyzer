using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using WebScraper.WebScrapers;

namespace WebScraper.Parsers
{
    public interface IDexTableParser
    {
        public IElement GetTable(IHtmlDocument page);
        public DexRow ParseRow(IElement rowHtml);
    }
}