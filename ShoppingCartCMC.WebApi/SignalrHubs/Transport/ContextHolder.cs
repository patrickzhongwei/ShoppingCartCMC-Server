using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCMC.WebApi.SignalrHubs.Transport
{
    public class ContextHolder : IContextHolder
    {
        public IHubCallerClients PricingHubClient { get; set; } //Patrick: add dynamic
    }
}
