using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.ReferenceData
{
    public abstract class CurrencyPairInfo
    {
        public CurrencyPairDto CurrencyPair { get; private set; }

        //Patrick: [todo] demo only; need to change code
        public decimal SampleRate { get; private set; }

        public bool Enabled { get; set; }
        public string Comment { get; set; }
        public bool Stale { get; set; }

        protected CurrencyPairInfo(CurrencyPairDto currencyPair, decimal sampleRate, bool enabled, string comment)
        {
            CurrencyPair = currencyPair;
            SampleRate = sampleRate;
            Enabled = enabled;
            Comment = comment;
        }

        public abstract PriceDto GenerateNextQuote(PriceDto lastPrice);
    }
}
