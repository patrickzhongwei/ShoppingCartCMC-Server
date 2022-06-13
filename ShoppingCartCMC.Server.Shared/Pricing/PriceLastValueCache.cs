using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Pricing
{
    public class PriceLastValueCache : IPriceLastValueCache
    {
        
        private readonly ConcurrentDictionary<string, PriceDto> _lastValueCache = new ConcurrentDictionary<string, PriceDto>();
       
        
        public PriceDto GetLastValue(string currencyPair)
        {
            PriceDto price;
            if (_lastValueCache.TryGetValue(currencyPair, out price))
            {
                return price;
            }
            throw new InvalidOperationException(string.Format("Currency pair {0} has not been initialized in last value cache", currencyPair));
        }

        public void StoreLastValue(PriceDto price)
        {
            _lastValueCache.AddOrUpdate(price.Symbol, _ => price, (s, p) => price);
        }
        

        //PW: we have to set a right value on PriceFeedSimulator.UpdatesPerSecond. Too fast may cause network problem; too slow may cause stale.
        //PW: so server won't finish all publishing in single Timer interval, we have to keep a index so that server, in next Timer interval, will publish from next index rather than from the begining.
        private object _lock = new object();
        private int _nextPublishIndex = 0;


        public string NextPublishCcyPair()
        {
            return this._lastValueCache.Keys.ElementAt(this._nextPublishIndex);
        }


        public void IncreaseNextPublishIndex()
        {
            if (this._lastValueCache.Keys.Count <= 0)
                return;

            lock (this._lock)
            {
                if (this._nextPublishIndex < (this._lastValueCache.Keys.Count - 1))
                    this._nextPublishIndex++;
                else
                    this._nextPublishIndex = 0;
            }
        }
    }
}
