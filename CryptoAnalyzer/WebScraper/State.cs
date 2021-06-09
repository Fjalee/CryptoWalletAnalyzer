using AngleSharp.Html.Dom;

namespace WebScraper
{
    public static class State
    {
        public static IHtmlDocument CurrentScrapingPageHtml { get; set; }

        public static string CurrentScrapingPageString
        {
            get
            {
                try
                {
                    return CurrentScrapingPageHtml.Children[0].OuterHtml;
                }
                catch
                {
                    System.Console.WriteLine("ERROR: Could not get OuterHtml of the page...");
                    return "";
                }
            }
        }
    }
}
