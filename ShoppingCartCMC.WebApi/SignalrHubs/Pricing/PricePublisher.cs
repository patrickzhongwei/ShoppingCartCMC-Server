using ShoppingCartCMC.WebApi.SignalrHubs.Transport;
using ShoppingCartCMC.Server.Shared;
using ShoppingCartCMC.Shared.DTO;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartCMC.Server.Shared.Pricing;

namespace ShoppingCartCMC.WebApi.SignalrHubs.Pricing
{
    public class PricePublisher : IPricePublisher
    {
        private readonly IContextHolder _contextHolder;
        private long _totalUpdatesPublished;

        public PricePublisher(IContextHolder contextHolder)
        {
            _contextHolder = contextHolder;
        }

        public async Task Publish(PriceDto price)
        {
            var context = _contextHolder.PricingHubClient;
            if (context == null) return;

            _totalUpdatesPublished++;
            var groupName = string.Format(PricingHub.PriceStreamGroupPattern, price.Symbol);

            try
            {
                await context.Group(groupName).SendAsync("OnNewPrice", price);               
            }
            catch (Exception e)
            {
                //Log.Error(string.Format("An error occurred while publishing price to group {0}: {1}", groupName, price), e);
            }
        }

        public long TotalPricesPublished { get { return _totalUpdatesPublished; } }
    }
}
