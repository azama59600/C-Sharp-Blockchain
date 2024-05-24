using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChainExample
{
    [Serializable]
    public class Transaction
    {
        public string From { get; }
        public string To { get; }
        public double Amount { get; }
        public Transaction(string from, string to, double amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}
