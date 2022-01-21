using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        private readonly OutputOptions _config;

        public DexCsvOutput(IOptions<OutputOptions> config)
        {
            _config = config.Value;
        }

        public void DoOutput(string outputName, DexTableOutputDto table, string timeElapsed, int nmRows)
        {
            var pathName = _config.Path;
            var fullPath = pathName + '/' + outputName + ".csv";

            Directory.CreateDirectory(pathName);

            try
            {
                using (var writer = new StreamWriter(fullPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteField("sep=,");
                    csv.NextRecord();
                    csv.Context.RegisterClassMap<DexRowOutputDtoMap>();
                    csv.WriteField("Time elapsed: ");
                    csv.WriteField(timeElapsed);
                    csv.WriteField("");
                    csv.WriteField("Rows scraped: ");
                    csv.WriteField(nmRows);
                    csv.NextRecord();
                    csv.WriteField("Token Name: ");
                    csv.WriteField(table.TokenName);
                    csv.WriteField("");
                    csv.NextRecord();
                    csv.NextRecord();
                    csv.WriteRecords(table.Rows);
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
