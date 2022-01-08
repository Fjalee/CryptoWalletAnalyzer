using AngleSharp.Dom;

namespace WebScraper.Parsers
{
    public interface IParserCommon
    {
        void StepIfMatches(ref IElement current, string v, string className, IElement element);
    }
}