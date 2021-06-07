using CsvHelper.Configuration;
using System.Globalization;

namespace CryptoAnalyzer
{
    public class TokenOutputDtoMap : ClassMap<TokenOutputDto>
    {
        public TokenOutputDtoMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.TotalInaccurateValue).Convert(x => x.Value.TotalInaccurateValue.ToString("0." + new string('#', 339)));
            Map(m => m.TotalValue).Convert(x => x.Value.TotalValue.ToString("0." + new string('#', 339)));
        }
    }
}
