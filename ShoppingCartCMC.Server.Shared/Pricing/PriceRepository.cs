using ShoppingCartCMC.Server.Shared.Pricing;
using ShoppingCartCMC.Shared.DTO;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ShoppingCartCMC.Server.Shared.Pricing
{
    public class PriceFeedSubscriber : IPriceFeed, IDisposable
    {
        private const int MinPeriodMilliseconds = 15;   //Timer resolution is about 15ms
        private const int UpdatesPerSecond = 15; //PW: add; Use this value to control how frequent server publish.

        private readonly IRedisClientsManager _redisManager; //PW: redis
        private readonly IPricePublisher _pricePublisher;
        private readonly IPriceLastValueCache _priceLastValueCache;

        private Timer _timer;
        private int _updatesPerTick = 1;
        private double _updatesPerSecond;

        public PriceFeedSubscriber(
            IPricePublisher pricePublisher,
            IPriceLastValueCache priceLastValueCache,
            IRedisClientsManager redisManager)
        {
            _pricePublisher = pricePublisher;
            _priceLastValueCache = priceLastValueCache;
            _redisManager = redisManager;
        }

        public void Start()
        {
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
                _updatesPerTick = (int)(MinPeriodMilliseconds / periodMs); //Patrick：here tick 15ms
                periodMs = MinPeriodMilliseconds;
            }
            else
            {
                _updatesPerTick = 1;
            }

            //_timer = new Timer(OnTimerTick, null, 50, 20000); //PW: test only!!!!!!
            _timer = new Timer(OnTimerTick, null, (int)periodMs, (int)periodMs);
        }

        public double GetUpdateFrequency()
        {
            return _updatesPerSecond;
        }


        private void OnTimerTick(object state)
        {
            try
            {               

                //PW: get price from redis
                IList<PriceDto> list;
                using (var redis = this._redisManager.GetClient())
                {
                    var redisPriceDto = redis.As<PriceDto>();
                    list = redisPriceDto.GetAll();
                }

                //PW: populate into _priceLastValueCache first
                foreach (PriceDto p in list)
                {
                    _priceLastValueCache.StoreLastValue(p);
                }

                //PW: publish prices which allocated according to UpdatesPerSecond value.
                for (int i = 0; i < _updatesPerTick; i++)
                {
                    var nextPrice = _priceLastValueCache.GetLastValue(_priceLastValueCache.NextPublishCcyPair());

                    _pricePublisher.Publish(nextPrice);

                    //PW: test
                    System.Diagnostics.Debug.WriteLine(nextPrice.Symbol + " " + nextPrice.Bid + " " + nextPrice.Ask);

                    _priceLastValueCache.IncreaseNextPublishIndex(); //PW: increase index after successfully published.
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {
            using (_timer)
            { }
        }
    }
}
