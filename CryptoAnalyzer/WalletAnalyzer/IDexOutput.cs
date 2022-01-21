namespace WalletAnalyzer
{
    public interface IDexOutput
    {
        public void DoOutput(string outputName, DexTableOutputDto table, string timeElapsed, int nmRows);
    }
}
