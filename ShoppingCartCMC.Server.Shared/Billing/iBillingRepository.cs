﻿using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartBilling = ShoppingCartCMC.Shared.Billing;

namespace ShoppingCartCMC.Server.Shared.Billing
{
    public interface iBillingRepository
    {
        /// <summary>
        /// place order
        /// </summary>
        /// <param name="billing">a Billing object for waiting for processing</param>
        /// <returns>order number generated by server</returns>
        Task<int> PlaceOrder(iBillingDto billing);
    }
}
