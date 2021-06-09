using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebScraper;

namespace CryptoAnalyzer
{
    public class Output
    {
        public List<TokenOutputDto> Append(List<TokenOutputDto> current, List<Transaction> newTxns)
        {
            foreach (var txn in newTxns)
            {
                var existingToken = current.FirstOrDefault(x => x.Token.CompareTo(txn.Token) == 0);
                if (existingToken == null)
                {
                    current.Add(new TokenOutputDto(txn.Token, txn.Known, txn.ValueInfo));
                }
                else
                {
                    if(existingToken.Known != txn.Known)
                    {
                        State.ExitAndLog(new StackTrace());
                    }
                    else
                    {
                        existingToken.AppendValue(txn.ValueInfo);
                    }
                }
            }
            return current;
        }
    }
}
