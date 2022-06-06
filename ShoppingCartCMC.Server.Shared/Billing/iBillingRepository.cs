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
        bool PlaceOrder(CartBilling billing);
    }
}
