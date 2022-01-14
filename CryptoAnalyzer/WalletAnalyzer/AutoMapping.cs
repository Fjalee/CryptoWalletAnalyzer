using AutoMapper;
using CryptoAnalyzer;
using WebScraper.WebScrapers;

namespace WalletAnalyzer
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<DexRow, DexOutputDto>();
        }
    }
}
