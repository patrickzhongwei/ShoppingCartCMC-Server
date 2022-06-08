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
    //[Authorize(AuthenticationSchemes = "Bearer")]  //PW: further development
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly iProductRepository _productRepository;
        private readonly iBillingRepository _billingRepository;
        private readonly iForexEngineRepository _forexEngineRepository;
        private readonly iShippingRepository _shippingRepository;

        public ShippingController(iProductRepository productRepository, iBillingRepository billingRepository, iForexEngineRepository forexEngineRepository, iShippingRepository shippingRepository)
        {
            _productRepository = productRepository;
            _billingRepository = billingRepository;
            _forexEngineRepository = forexEngineRepository;
            _shippingRepository = shippingRepository;
        }


        /// <summary>
        /// get shipping fee by cartSumPrice and ccyCode
        /// </summary>
        /// <param name="cartSumPrice">shopping card sum price</param>
        /// <param name="ccyCode">ccy code</param>
        /// <returns></returns>
        [HttpGet()] //PW: don't go that way, or Angualr cannot handle it // [HttpGet("{cartSumPrice}/{ccyCode}")]
        public async Task<decimal> Get(decimal cartSumPrice, string ccyCode = "AUD")
        {
            return await _shippingRepository.GetShippingFee(cartSumPrice, ccyCode);
        }

    }
}
