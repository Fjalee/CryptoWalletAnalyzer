using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebScraper
{
    public class EtherscanApiServices : IEtherscanApiServices
    {
        private readonly Uri _baseUri;
        private readonly string _apiKey;

        const int _secondToMs = 1000;
        private readonly int _apiCallsAllowedPerSecond;
        private readonly int _apiTryAgainDelay;
        private int _apiCallsLeft;
        private readonly Stopwatch _msSinceApiLimitStarted = new Stopwatch();
        private readonly Stopwatch _tempSw = new Stopwatch();

        public EtherscanApiServices(IConfiguration config)
        {
            _tempSw.Start();

            var apiUrl = config.GetSection("BLOCKCHAINS").GetSection("ETHERSCAN").GetSection("API")["PATH"];
            _baseUri = new Uri(apiUrl);
            _apiKey = "api_key"; // fix, this should be kept as github secret

            try
            {
                _apiCallsAllowedPerSecond = config
                    .GetSection("BLOCKCHAINS").GetSection("ETHERSCAN").GetSection("API").GetSection("CALLS-PER-SECOND")
                    .Get<Int32>();
                _apiTryAgainDelay = config
                    .GetSection("BLOCKCHAINS").GetSection("ETHERSCAN").GetSection("API").GetSection("TRY-AGAIN-DELAY-IN-MS")
                    .Get<Int32>();
            }
            catch
            {
                State.ExitAndLog(new StackTrace());
            }

            _apiCallsLeft = _apiCallsAllowedPerSecond;
            _msSinceApiLimitStarted.Start();
        }

        public async Task<TransactionDetails> GetTransactionDetailsAsync(string txnHash)
        {
            WaitIfNeeded();
            _apiCallsLeft--;

            var client = CreateClientForRequest();
            return await MakeRequestsUntilGet(client, txnHash);
        }

        private HttpClient CreateClientForRequest()
        {
            var client = new HttpClient
            {
                BaseAddress = _baseUri
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private async Task<string> GetJsonResponse(HttpClient client, string txnHash)
        {
            var parameters = $"?module=proxy&action=eth_getTransactionByHash&txhash={txnHash}&apikey={_apiKey}";
            var response = await client.GetAsync(parameters);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"From Etherscan API couldn't get transaction {txnHash}");
                return null;
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        private async Task<TransactionDetails> MakeRequestsUntilGet(HttpClient client, string txnHash)
        {
            var jsonResponse = await GetJsonResponse(client, txnHash);

            TransactionDetails result = null;
            try
            {
                result = JsonConvert.DeserializeObject<EtherscanTransactionByHashSuccessfulResponse>(jsonResponse).Result;
            }
            catch (JsonSerializationException)
            {
                var failReason = JsonConvert.DeserializeObject<EtherscanTransactionByHashFailedResponse>(jsonResponse).Result;
                switch (failReason)
                {
                    case "Invalid API Key":
                        Console.WriteLine($"ERROR, API response: \"{failReason}\"");
                        State.ExitAndLog(new StackTrace());
                        break;
                    case "Max rate limit reached":
                        result = HandleRequestLimitReached(client, txnHash);
                        break;
                    default:
                        Console.WriteLine($"Unexpected API fail response: \"{failReason}\"");
                        State.ExitAndLog(new StackTrace());
                        break;
                }
            }
            catch (Exception ex)
            {
                State.ExitAndLog(new StackTrace());
            }

            return result;
        }

        private TransactionDetails HandleRequestLimitReached(HttpClient client, string txnHash)
        {
            Task.Delay(_apiTryAgainDelay).Wait();
            var result = MakeRequestsUntilGet(client, txnHash).Result;
            _apiCallsLeft = _apiCallsAllowedPerSecond - 2; // just for safety since we don't know how long ti took for api to respond
            _msSinceApiLimitStarted.Restart();
            return result;
        }

        private void WaitIfNeeded()
        {
            if (_apiCallsLeft <= 0)
            {
                var msToWait = _secondToMs - (int)_msSinceApiLimitStarted.ElapsedMilliseconds;
                msToWait = msToWait < 0 ? 0 : msToWait;
                Task.Delay(msToWait).Wait();

                _apiCallsLeft = _apiCallsAllowedPerSecond;
                _msSinceApiLimitStarted.Restart();
            }
        }
    }
}
