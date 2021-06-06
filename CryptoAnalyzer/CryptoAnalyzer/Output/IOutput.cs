using System.Collections.Generic;

namespace CryptoAnalyzer
{
    interface IOutput
    {
        public void CreateFile(string pathName, string fileName, List<TokenOutputDto> list);
    }
}
