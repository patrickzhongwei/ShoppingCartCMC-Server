using Microsoft.Extensions.Logging;
using ShoppingCartCMC.Server.Shared.BizRule;
using ShoppingCartCMC.Server.Shared.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Shipping
{
    public class ShippingRepository : iShippingRepository
    {
        private readonly ILogger<ShippingRepository> _logger;
        private readonly iForexEngineRepository _forexEngineRepository;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="forexEngineRepository"></param>
        public ShippingRepository(ILogger<ShippingRepository> logger, iForexEngineRepository forexEngineRepository)
        {
            _logger = logger;
            _forexEngineRepository = forexEngineRepository;
        }


        /// <summary>
        /// Get shipping fee by cart sumPrice and ccyCode
        /// </summary>
        /// <param name="cartSumPrice">shopping cart sum price</param>
        /// <param name="ccyCode">currency code</param>
        /// <returns>amount of shipping fee</returns>
        public decimal GetShippingFee(decimal cartSumPrice, string ccyCode)
        {
            decimal shippingRate1 = 10M;
            decimal shippingRate2 = 20M;
            decimal thresholdInBaseCcy = 50M;
            decimal thresholdInCcyCode = thresholdInBaseCcy;

            //PW: calulate by different ccyCode
            if (ccyCode != ShippingRule.CartBaseCcy)
            {
                decimal directRate = this._forexEngineRepository.GetDirectRate(ShippingRule.CartBaseCcy + ccyCode);

                shippingRate1 = Math.Round(shippingRate1 / directRate, 0);
                shippingRate2 = Math.Round(shippingRate2 / directRate, 0);
                thresholdInCcyCode = Math.Round(thresholdInBaseCcy / directRate, 0);
            }

            //PW: compare with threshold
            if (cartSumPrice < thresholdInCcyCode)
                return shippingRate1;
            else
                return shippingRate2;

        }
    }
}
