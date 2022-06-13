using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Pricing
{
    public interface IPriceRepository 
    {
        void Init();

        PriceDto GetPrice(string currencyPair);
    

        void SubscribePrice(string ccyPair, string connectionId);        

        void UnsubscribePrice(string ccyPair, string connectionId);

        void StoreBestPriceBatch();
                
        string NextSubscriptionDictionaryKey(); 

        void IncreaseNextSubscriptionIndex(); 


    }
}
