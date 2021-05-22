using System.Threading.Tasks;

namespace WebScraper
{
    public class Program
    {

        private const string _siteUrl = "https://bscscan.com/tokentxns"; //fix temp put into config file

        static async Task Main()
        {
            var scrapedPage = await new TempName().GetIHtmlDoc(_siteUrl);

            var paraser = new Parser();
            var txnTable = paraser.ParseTxnTable(scrapedPage);
        }
    }
}
