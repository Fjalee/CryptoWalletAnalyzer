using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Threading.Tasks;
using WebScraper.WebScrapers;

namespace WebScraper.Parsers
{
    public interface IDexTableParser
    {
        IElement GetTable(IHtmlDocument page);
        Task<DexRow> ParseRow(IElement rowHtml); //fix maybe Parse method shouldnt be async
    }
}