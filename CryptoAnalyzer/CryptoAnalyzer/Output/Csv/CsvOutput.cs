using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CryptoAnalyzer
{
    public class CsvOutput : IOutput
    {
        public void CreateFile(string pathName, string fileName, List<TokenOutputDto> list)
        {
            var fullPath = pathName + '/' + fileName + ".csv";


            Directory.CreateDirectory(pathName);

            try
            {
                using (var writer = new StreamWriter(fullPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<TokenOutputDtoMap>();
                    csv.WriteRecords(list);
                }
            }
            catch (IOException e)
            {
                //fix temp add console text
            }
            catch
            {
                throw;
            }
        }
    }
}
