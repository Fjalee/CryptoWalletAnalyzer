using System.Configuration;
using System.Threading.Tasks;

namespace WebScraper
{
    public class Program
    {
        private readonly string _domainUrl = ConfigurationManager.AppSettings.Get("DOMAIN_NAME");
        private readonly string _path = ConfigurationManager.AppSettings.Get("PATH");

        static async Task Main()
        {
            //var scrapedPage = await new TempName().GetIHtmlDoc(_domainUrl + "/" + _path);

            //var paraser = new Parser(_domainUrl);
            //var txnTable = paraser.ParseTxnTable(scrapedPage);

            //var allTxn = new List<Transaction>();
            //foreach (var row in txnTable.Children)
            //{
            //    allTxn.Add(await paraser.ParseTxnRow(row));
            //}
        }
    }
}
