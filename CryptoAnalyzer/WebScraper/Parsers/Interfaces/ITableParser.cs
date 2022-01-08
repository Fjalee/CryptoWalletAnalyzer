using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace WebScraper.Parsers
{
    public interface ITableParser
    {
        public IElement GetTable(IHtmlDocument page);
        public IRow ParseRow(IElement rowHtml);
    }
}