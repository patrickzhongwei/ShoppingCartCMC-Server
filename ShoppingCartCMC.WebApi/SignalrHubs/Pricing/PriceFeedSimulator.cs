using ShoppingCartCMC.Server.Shared.Pricing;
using ShoppingCartCMC.Server.Shared.ReferenceData;
using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCartCMC.WebApi.SignalrHubs.Pricing
{
    public class PriceFeedSimulator : IPriceFeed, IDisposable
    {
        private const int MinPeriodMilliseconds = 15;   //Timer resolution is about 15ms
        private const int UpdatesPerSecond = 15; //PW: add; Use this value to control how frequent server publish.

        private readonly ICurrencyPairRepository _currencyPairRepository;
        private readonly IPricePublisher _pricePublisher;
        private readonly IPriceLastValueCache _priceLastValueCache;
        private readonly Random _random;
        private Timer _timer;
        private int _updatesPerTick = 1;
        private double _updatesPerSecond;

        public PriceFeedSimulator(           
            IPricePublisher pricePublisher,
            IPriceLastValueCache priceLastValueCache)
        {         
            _pricePublisher = pricePublisher;
            _priceLastValueCache = priceLastValueCache;
        }

        public void Start()
        {
            PopulateLastValueCache();
            SetUpdateFrequency(UpdatesPerSecond);
        }

        public void SetUpdateFrequency(double updatesPerSecond)
        {
            _updatesPerSecond = updatesPerSecond;
            if (_timer != null)
            {
                _timer.Dispose();
            }

            var periodMs = 1000.0 / updatesPerSecond;

            if (periodMs < (MinPeriodMilliseconds + 1)) // Instead of trying to fire more often than timer resolution allows, start pushing more updates per tick.
            {
                _updatesPerTick = (int)(MinPeriodMilliseconds / periodMs); //Patrick：这里tick间隔是15ms
                periodMs = MinPeriodMilliseconds;
            }
            else
            {
                _updatesPerTick = 1;
            }

            _timer = new Timer(OnTimerTick, null, (int)periodMs, (int)periodMs);
        }

        public double GetUpdateFrequency()
        {
            return _updatesPerSecond;
        }

        private void PopulateLastValueCache()
        {
            //foreach (var currencyPairInfo in _currencyPairRepository.GetAllCurrencyPairInfos())
            //{
            //    var mid = _currencyPairRepository.GetSampleRate(currencyPairInfo.CurrencyPair.Symbol);

            //    var initialQuote = new PriceDto
            //    {
            //        Symbol = currencyPairInfo.CurrencyPair.Symbol,
            //        Mid = mid
            //    };

            //    _priceLastValueCache.StoreLastValue(currencyPairInfo.GenerateNextQuote(initialQuote));
            //}
        }

        private void OnTimerTick(object state)
        {
            var activePairs = _currencyPairRepository.GetAllCurrencyPairInfos().Where(cp => cp.Enabled && !cp.Stale).ToList();

            if (activePairs.Count == 0)
                return;

            for (int i = 0; i < _updatesPerTick; i++)
            {
                var randomCurrencyPairInfo = activePairs[_random.Next(0, activePairs.Count)];
                var lastPrice = _priceLastValueCache.GetLastValue(randomCurrencyPairInfo.CurrencyPair.Symbol);

                var newPrice = randomCurrencyPairInfo.GenerateNextQuote(lastPrice);              

                _priceLastValueCache.StoreLastValue(newPrice);
                _pricePublisher.Publish(newPrice);
            }
        }

        public void Dispose()
        {
            using (_timer)
            { }
        }        
    }
}
