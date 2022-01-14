using CryptoAnalyzer;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using WebScraper;
using WebScraper.Parsers;

namespace WalletAnalyzer
{
    public class DexCsvOutput : IDexOutput
    {
        private readonly IConfiguration _config;

        public DexCsvOutput(IConfiguration config)
        {
            _config = config;
        }

        public void DoOutput(string outputName, List<DexOutputDto> list, string timeElapsed, int nmRows)
        {
            var pathName = _config.GetSection("OUTPUT")["PATH"];
            var fullPath = pathName + '/' + outputName + ".csv";

            Directory.CreateDirectory(pathName);

            try
            {
                using (var writer = new StreamWriter(fullPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteField("sep=,");
                    csv.NextRecord();
                    csv.Context.RegisterClassMap<DexOutputDtoMap>();
                    csv.WriteField("Time elapsed: ");
                    csv.WriteField(timeElapsed);
                    csv.WriteField("");
                    csv.WriteField("Rows scraped: ");
                    csv.WriteField(nmRows);
                    csv.NextRecord();
                    csv.NextRecord();
                    csv.WriteRecords(list);
                }
            }
            catch (IOException)
            {
                throw;
            }
            catch
            {
                State.ExitAndLog(new StackTrace());
            }
        }
    }
}
