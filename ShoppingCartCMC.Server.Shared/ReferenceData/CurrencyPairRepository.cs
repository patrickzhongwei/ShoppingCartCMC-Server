using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.ReferenceData
{
    public class CurrencyPairRepository : ICurrencyPairRepository
    {
        private readonly Dictionary<string, CurrencyPairInfo> _currencyPairs = new Dictionary<string, CurrencyPairInfo>
        {           
            {"AUDUSD", CreateCurrencyPairInfo("AUDUSD", 4, 5, 0.7202m, true)},
            {"AUDNZD", CreateCurrencyPairInfo("AUDNZD", 4, 5, 1.1101m, true)},
        };

        private static CurrencyPairInfo CreateCurrencyPairInfo(string symbol, int pipsPosition, int ratePrecision, decimal sampleRate, bool enabled, string comment = "")
        {
            return new RandomWalkCurrencyPairInfo(new CurrencyPairDto(symbol, ratePrecision, pipsPosition), sampleRate, enabled, comment);
        }
                

        public IEnumerable<CurrencyPairInfo> GetAllCurrencyPairInfos()
        {
            return _currencyPairs.Values;
        }

        public CurrencyPairDto GetCurrencyPair(string symbol)
        {
            return _currencyPairs[symbol].CurrencyPair;
        }


        //Patrick: [todo] demo only; need to change code
        public decimal GetSampleRate(string symbol)
        {
            return _currencyPairs[symbol].SampleRate;
        }

        public bool Exists(string symbol)
        {
            return _currencyPairs.ContainsKey(symbol);
        }
    }
}
