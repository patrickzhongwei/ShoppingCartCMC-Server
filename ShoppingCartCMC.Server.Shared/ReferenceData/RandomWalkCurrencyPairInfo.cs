﻿using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.ReferenceData
{
    public sealed class RandomWalkCurrencyPairInfo : CurrencyPairInfo
    {
        private static readonly Random Random = new Random();
        private readonly int _halfSpread;

        public RandomWalkCurrencyPairInfo(CurrencyPairDto currencyPair, decimal sampleRate, bool enabled, string comment)
            : base(currencyPair, sampleRate, enabled, comment)
        {
            _halfSpread = Random.Next(2, 10);
        }

        public override PriceDto GenerateNextQuote(PriceDto previousPrice)
        {
            var pow = (decimal)Math.Pow(10, CurrencyPair.RatePrecision);
            var newMid = previousPrice.Mid + Random.Next(-5, 5) / pow;

            // check that the new mid does not drift too far from sampleRate (3%)
            if (Math.Abs(newMid - SampleRate) / SampleRate > .03m)
            {
                newMid = SampleRate;
            }


            return new PriceDto
            {
                Symbol = previousPrice.Symbol,                
                Ask = newMid + _halfSpread / pow,
                Bid = newMid - _halfSpread / pow,
            };
        }
    }
}
