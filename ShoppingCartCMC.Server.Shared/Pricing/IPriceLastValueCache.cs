using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Pricing
{
    public interface IPriceLastValueCache
    {
        PriceDto GetLastValue(string currencyPair);
        void StoreLastValue(PriceDto price);
        string NextPublishCcyPair(); //PW: add
        void IncreaseNextPublishIndex(); //PW: add
    }
}
