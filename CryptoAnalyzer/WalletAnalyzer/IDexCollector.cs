using System.Threading.Tasks;

namespace WalletAnalyzer
{
    public interface IDexCollector
    {
        Task Start(string url, int sleepTimeMs, int appendPeriodInMs);
    }
}