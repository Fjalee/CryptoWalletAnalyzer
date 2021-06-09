using AngleSharp.Html.Dom;

namespace WebScraper
{
    public static class State
    {
        public static IHtmlDocument CurrentScrapingPage { get; set; }
    }
}
