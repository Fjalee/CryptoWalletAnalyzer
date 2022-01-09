using CryptoAnalyzer;
using System.Collections.Generic;

namespace WalletAnalyzer
{
    public interface IDexOutput
    {
        public void DoOutput(string outputName, List<DexOutputDto> list, string timeElapsed, int nmRows);
    }
}
