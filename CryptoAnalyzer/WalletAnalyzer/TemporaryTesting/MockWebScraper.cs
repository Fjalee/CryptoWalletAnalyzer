using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebScraper.WebScrapers;

namespace WalletAnalyzer.TemporaryTesting
{
    public class MockWebScraper : IWebScraper
    {
        public async Task<IHtmlDocument> GetPage(string url)
        {
            string pageString = "<html lang=\"en\"><head><title>Etherscan Error Page</title><metaname=\"viewport\"content=\"width=device-width, initial-scale=1, shrink-to-fit=no\"/><metaname=\"Description\"content=\"The Ethereum BlockChain Explorer, API and Analytics Platform\"/><meta name=\"author\" content=\"etherscan.io\" /><metaname=\"keywords\"content=\"ethereum, explorer, ether, search, blockchain, crypto, currency\"/><meta name=\"format-detection\" content=\"telephone=no\" /><link rel=\"shortcut icon\" href=\"/images/favicon2.ico\" /><linkrel=\"stylesheet\"href=\"/assets/vendor/font-awesome/css/fontawesome-all.min.css?v=21.12.4.1\"/><link rel=\"stylesheet\" href=\"/assets/css/theme.min.css?v=21.12.4.1\" /><script async=\"\" src=\"/cdn-cgi/bm/cv/669835187/api.js\"></script></head><body><header id=\"header\" class=\"u-header\"><div class=\"u-header__section\"><div id=\"logoAndNav\" class=\"container\"><navclass=\"js-mega-menu navbar navbar-expand-md u-header__navbar u-header__navbar--no-space 1d-block\"><div><aclass=\"navbar-brand\"href=\"/\"target=\"_parent\"aria-label=\"Etherscan\"><script type=\"text/javascript\" style=\"display: none\">//<![CDATA[window.__mirage2 = {petok:\"425f17787f13877fbfe6deeb816600d1e47162be-1642175849-1800\"};//]]></script><scripttype=\"text/javascript\"src=\"https://ajax.cloudflare.com/cdn-cgi/scripts/04b3eb47/cloudflare-static/mirage2.min.js\"></script><imgwidth=\"160\"data-cfsrc=\"/images/logo-ether.png?v=0.0.2\"alt=\"Etherscan Logo\"style=\"display: none; visibility: hidden\"/><noscript><imgwidth=\"160\"src=\"/images/logo-ether.png?v=0.0.2\"alt=\"Etherscan Logo\" /></noscript></a></div></nav></div></div></header><mainid=\"content\"class=\"bg-img-hero-center\"style=\"background-image: url(images/error-404.svg)\"role=\"main\"><div class=\"container d-lg-flex align-items-lg-center min-height-100vh\"><div class=\"w-lg-60 w-xl-50\"><div class=\"mb-5\"><h1 class=\"text-secondary font-weight-normal\"><span class=\"font-weight-semi-bold mr-2\">Sorry!</span>Weencountered an unexpected error.</h1><p class=\"mb-0\">An unexpected error occurred. <br />Please check back later</p></div><a class=\"btn btn-primary btn-wide transition-3d-hover\" href=\"/\">Back Home</a></div></div></main><script type=\"text/javascript\">(function () {window[\"__CF$cv$params\"] = {r: \"6cd80f6efe62165a\",m: \"3vJ0eCcAdKxUSEffYN8Uebhuh9vQ3DFLeN0pN.gYhGM-1642175849-0-AVOiMZ5cmbzvPB5rnFtJVQkyBkyMAW9sSjq8GiZCXq7HFGr+/7b+Kto0z/mlnSpS0MnxUDEEcRelUhXImHt4VEm3AiuGl/ApDk2uJoXfox1EJ+SEExIj1pUvm+wvrZ+Zcg==\",s: [0x14330b8b09, 0x16c39db9e8],};})();</script></body></html>";
            var parser = new HtmlParser();

            return parser.ParseDocument(pageString);
        }
    }
}
