using System.Collections.Generic;

namespace CryptoAnalyzer
{
    interface IOutput
    {
        public void WriteFile(string pathName, string fileName, List<TokenOutputDto> list, string timeElapsed);
    }
}
