using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.MarketData
{ 
    public interface iForexEngineRepository
    {
        /// <summary>
        /// Get market rate by ccyPair
        /// </summary>
        /// <param name="ccyPair">currency pair</param>
        /// <returns>direct rate </returns>
        Task<decimal> GetIndirectRate(string ccyPair);
    }
}
