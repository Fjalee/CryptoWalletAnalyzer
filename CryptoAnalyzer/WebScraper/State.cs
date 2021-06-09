using AngleSharp.Html.Dom;
using System;
using System.Diagnostics;

namespace WebScraper
{
    public static class State
    {
        public static IHtmlDocument CurrentScrapingPageHtml { get; set; }
        public static string ScrapeDate { get; set; }

        public static void ExitAndLog(StackTrace stackTrace)
        {
            var methodName = stackTrace.GetFrame(0).GetMethod().Name;

            try
            {
                Trace.WriteLine("\nScrape date: " + ScrapeDate + "\n" + CurrentScrapingPageHtml.Children[0].OuterHtml);
                Console.WriteLine("\nERROR: Something went wrong in method " + methodName + "\nHtml page was printed into the log file...");
            }
            catch
            {
                Console.WriteLine("\nERROR: Could not get OuterHtml of the page...");
            }

            Console.WriteLine("Press any key to exit...");
            Console.Read();
            Environment.Exit(0);
        }
    }
}
