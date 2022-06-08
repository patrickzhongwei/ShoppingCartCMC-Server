using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ShoppingCartCMC.Server.Shared.Product;
using ShoppingCartCMC.Server.Shared.Billing;
using ShoppingCartCMC.Server.Shared.MarketData;
using ShoppingCartCMC.Server.Shared.Shipping;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCartCMC.WebApi.Controllers
{

    //[Authorize(AuthenticationSchemes = "Bearer")]  //PW: further development[Route("api/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private readonly iProductRepository _productRepository;
        private readonly iBillingRepository _billingRepository;
        private readonly iForexEngineRepository _forexEngineRepository;
        private readonly iShippingRepository _shippingRepository;

        public MarketController(iProductRepository productRepository, iBillingRepository billingRepository, iForexEngineRepository forexEngineRepository, iShippingRepository shippingRepository)
        {
            _productRepository = productRepository;
            _billingRepository = billingRepository;
            _forexEngineRepository = forexEngineRepository;
            _shippingRepository = shippingRepository;
        }

        /** *
         * Patrick: [not used].
         */
        /// <summary>
        /// get forex rate, it is a indirect rate like: AUDUSD, AUDNZD, note: AUD is base currency.
        /// </summary>
        /// <param name="ccyPair">currency pair</param>
        /// <returns>market rate</returns>
        //[HttpGet()] //PW: don't go that way, or Angualr cannot handle it // [HttpGet("{cartSumPrice}/{ccyCode}")]
        //public async Task<decimal> Get(string ccyPair)
        //{
        //    return await _forexEngineRepository.GetIndirectRate(ccyPair);
        //}
    }
}
