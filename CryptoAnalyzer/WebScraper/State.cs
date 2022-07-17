using AngleSharp.Html.Dom;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace WebScraper
{
    public static class State
    {
        public static IHtmlDocument CurrentScrapingPageHtml { get; set; }
        public static string ScrapeDate { get; set; }
        public static void ExitAndLog(StackTrace stackTrace, ILogger logger)
        {
            var methodName = stackTrace.GetFrame(0).GetMethod().Name;
            var outerHtml = "";
            var logMessage = "\nScrape date: " + ScrapeDate + "\nSomething went wrong in method " + methodName +
                ", OuterHtml will be printed out in the log file.";

            try
            {
                outerHtml  = CurrentScrapingPageHtml.Children[0].OuterHtml;
            }
            catch
            {
                logMessage += "\nCould not get OuterHtml of the page.";
            }

            logger.LogCritical(logMessage + "\n" + outerHtml + "\n\n\n");

            logger.LogInformation("Press any key to exit...");
            Console.Read();
            Environment.Exit(0);
        }
    }
}
