using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Billing
{
    public class BilliingRepository : iBillingRepository
    {
        private readonly ILogger<BilliingRepository> _logger;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        public BilliingRepository(ILogger<BilliingRepository> logger)
        {
            _logger = logger;
        }


    /// <summary>
    /// Place orer
    /// </summary>
    /// <param name="billing"></param>
    /// <returns></returns>
    public bool PlaceOrder(ShoppingCartCMC.Shared.Billing billing)
        {
            /** *
            * Patrick: [todo in future].
            * validae billing first, persist into db, generate billing receipt 
            */
            return true;
        }
    }
}
