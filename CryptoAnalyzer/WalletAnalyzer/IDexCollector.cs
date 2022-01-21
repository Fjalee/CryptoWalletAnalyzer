using System.Threading.Tasks;

namespace WalletAnalyzer
{
    public interface IDexCollector
    {
        Task Start(string url, string tokenHash, int sleepTimeMs, int appendPeriodInMs, int nmRowsToScrape);
    }
}