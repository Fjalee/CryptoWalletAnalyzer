using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WalletAnalyzer
{
    public class EtherscanApiServices : IEtherscanApiServices
    {
        private readonly Uri _baseUri;
        private readonly string _apiKey;

        public EtherscanApiServices(IConfiguration config)
        {
            var apiUrl = config.GetSection("BLOCKCHAINS").GetSection("ETHERSCAN")["API"];
            _baseUri = new Uri(apiUrl);
            _apiKey = "my_api_key"; // fix, this should be kept as github secret
        }

        public async Task<TransactionDetails> GetTransactionDetailsAsync(string txnHash)
        {
            var parameters = $"?module=proxy&action=eth_getTransactionByHash&txhash={txnHash}&apikey={_apiKey}";

            var client = new HttpClient
            {
                BaseAddress = _baseUri
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(parameters);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"From Etherscan API couldn't get transaction {txnHash}");
                return null;
            }

            TransactionDetails result = null;
            try
            {
                result = response.Content.ReadAsAsync<EtherscanTransactionByHashResponse>().Result.Result;
            }
            catch
            {
                Console.WriteLine($"Couldn't deserialize Etherscan API response");
            }

            return result;
        }
    }
}
