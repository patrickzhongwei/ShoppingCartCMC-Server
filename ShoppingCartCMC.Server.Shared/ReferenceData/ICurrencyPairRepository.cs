using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.ReferenceData
{
    public interface ICurrencyPairRepository
    {
        CurrencyPairDto GetCurrencyPair(string symbol);
        bool Exists(string symbol);

        //Patrick: [todo] demo only, need change code
        decimal GetSampleRate(string symbol);

        IEnumerable<CurrencyPairInfo> GetAllCurrencyPairInfos();
    }
}
