using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.MarketData
{
    public class ForexEngineRepository : iForexEngineRepository
    {
        private readonly ILogger<ForexEngineRepository> _logger;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        public ForexEngineRepository(ILogger<ForexEngineRepository> logger)
        {
            _logger = logger;
        }



        /// <summary>
        /// Get market rate by ccyPair
        /// </summary>
        /// <param name="ccyPair">currency pair</param>
        /// <returns>direct rate </returns>
        public decimal GetDirectRate(string ccyPair)
        {
            //PW: mock-up static rate            
            /** *
            * Patrick: [todo in future].
            * rates need update dailly
            */
            if (ccyPair == "AUDNZD")
                return 0.92M;
            else if (ccyPair == "AUDUSD")
                return 0.60M;
            else
                return 1.0M;
        }
    }
}
