using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebScraper
{
    public class Program
    {

        private const string _domainUrl = "https://bscscan.com"; //fix temp put into config file
        private const string _path = "tokentxns"; //fix temp put into config file


        static async Task Main()
        {
            var scrapedPage = await new TempName().GetIHtmlDoc(_domainUrl + "/" + _path);

            var paraser = new Parser();
            var txnTable = paraser.ParseTxnTable(scrapedPage);

            var allTxn = new List<Transaction>();
            foreach (var row in txnTable.Children)
            {
                allTxn.Add(await paraser.ParseTxnRow(row));
            }
        }
    }
}
