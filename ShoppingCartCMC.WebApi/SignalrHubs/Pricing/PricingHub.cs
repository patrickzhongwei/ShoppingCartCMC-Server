using ShoppingCartCMC.WebApi.SignalrHubs.Transport;
using ShoppingCartCMC.Server.Shared;
using ShoppingCartCMC.Server.Shared.Common;
using ShoppingCartCMC.Server.Shared.Pricing;
using ShoppingCartCMC.Shared.Common;
using ShoppingCartCMC.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartCMC.Server.Shared.ReferenceData;
using SignalRSwaggerGen.Attributes;

namespace ShoppingCartCMC.WebApi.SignalrHubs.Pricing
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(Policy = "CmcSignalrApi_Policy")]
    [SignalRHub] //PW: Swagger for SignalR is not useless, as it generates Http post not working. 
    public class PricingHub : Hub
    {        
        private readonly IContextHolder _contextHolder;
        private readonly IPriceLastValueCache _priceLastValueCache;

        public const string PriceStreamGroupPattern = "Pricing/{0}";

        public PricingHub(
           
            IContextHolder contextHolder
            )
        {          
            _contextHolder          = contextHolder;
        }


        public async Task SubscribePriceStream(string ccyPair = "AUDUSD")
        {
            _contextHolder.PricingHubClient = Clients;
            //Log.InfoFormat("Received subscription request {0} from connection {1}", request, Context.ConnectionId);  

            //PW: (1)add client to groups
            var groupName = string.Format(PriceStreamGroupPattern, ccyPair);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);


            ////PW: (2)send current price to client
            //var lastValue = _priceLastValueCache.GetLastValue(ccyPair);
            //await Clients.Client(Context.ConnectionId).SendAsync("OnNewPrice", lastValue);

            //PW: test only
            await Clients.Client(Context.ConnectionId).SendAsync("OnNewPrice", 0.888M);
        }        

    }
}
