using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Shipping
{
    public interface iShippingRepository
    {

        /// <summary>
        /// Get shipping fee by cart sumPrice and ccyCode
        /// </summary>
        /// <param name="cartSumPrice">shopping cart sum price</param>
        /// <param name="ccyCode">currency code</param>
        /// <returns>amount of shipping fee</returns>
        Task<decimal> GetShippingFee(decimal cartSumPrice, string ccyCode);
    }
}
