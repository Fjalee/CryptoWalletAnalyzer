using CsvHelper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using WebScraper;

namespace CryptoAnalyzer
{
    public class CsvOutput : IOutput
    {
        public void WriteFile(string pathName, string fileName, List<TokenOutputDto> list, string timeElapsed, int nmTxnScraped)
        {
            var fullPath = pathName + '\\' + fileName + ".csv";

            Directory.CreateDirectory(pathName);

            try
            {
                using (var writer = new StreamWriter(fullPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<TokenOutputDtoMap>();
                    csv.WriteField("Time elapsed: ");
                    csv.WriteField(timeElapsed);
                    csv.WriteField("");
                    csv.WriteField("Transactions scraped: ");
                    csv.WriteField(nmTxnScraped);
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
