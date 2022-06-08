using Microsoft.Extensions.Logging;
using ShoppingCartCMC.Server.Shared.Common;
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
        /// Get indirect market rate by ccyPair, indirect means all currency pair must be started with domestic(base) currency, here AUD
        /// </summary>
        /// <param name="ccyPair">currency pair</param>
        /// <returns>direct rate </returns>
        public async Task<decimal> GetIndirectRate(string ccyPair)
        {
            try
            {
                /** *
                * Patrick: [todo in future].
                * PW: await CPU-bound work here...
                */

                //PW: mock-up static rate            
                /** *
                * Patrick: [todo in future].
                * rates need update dailly
                */
                if (ccyPair == "AUDNZD")
                    return 1.11M;
                else if (ccyPair == "AUDUSD")
                    return 0.72M;
                else
                {
                    var msg = "unsupported currency pair at GetDirectRate(...), " + ccyPair;
                    _logger.LogError(msg);
                    throw new CmcCartException(msg, ErrorCode.CCYPAIR_UNSUPPORT);
                }
            }
            catch (Exception ex)
            {
                //PW: logger ...
                return 0M;
            }
        }
    }
}
