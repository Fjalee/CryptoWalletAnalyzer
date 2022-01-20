namespace WebScraper
{
    public class AppSettingsOptions
    {
        public OutputOptions Output { get; set; }
        public BlockchainsOptions Blockchains { get; set; }
    }

    public class BlockchainsOptions
    {
        public BlockchainExplorerWebsiteOptions Etherscan { get; set; }
    }

    public class BlockchainExplorerWebsiteOptions
    {
        public string DomainName { get; set; }
        public ApiOptions Api { get; set; }
        public int SleepTimeBetweenScrapesInMs { get; set; }
    }   

    public class ApiOptions
    {
        public string Path { get; set; }
        public int CallsPerSecond { get; set; }
        public int TryAgainDelayInMs { get; set; }
        public string ApiKey { get; set; }
    }

    public class OutputOptions
    {
        public string Path { get; set; }
        public string LogPath { get; set; }
        public int AppendPeriodInSeconds { get; set; }
    }
}
