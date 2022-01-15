using System.Threading.Tasks;

namespace WalletAnalyzer
{
    public interface IEtherscanApiServices
    {
        Task<TransactionDetails> GetTransactionDetailsAsync(string txnHash);
    }
}