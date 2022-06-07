using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ShoppingCartCMC.Server.Shared.Product;
using ShoppingCartCMC.Server.Shared.Billing;
using ShoppingCartCMC.Server.Shared.MarketData;
using ShoppingCartCMC.Server.Shared.Shipping;
using ShoppingProduct = ShoppingCartCMC.Shared.Product;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCartCMC.WebApi.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]  //PW: further development
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly iProductRepository _productRepository;
        private readonly iBillingRepository _billingRepository;
        private readonly iForexEngineRepository _forexEngineRepository;
        private readonly iShippingRepository _shippingRepository;
        public ProductController(iProductRepository productRepository, iBillingRepository billingRepository, iForexEngineRepository forexEngineRepository, iShippingRepository shippingRepository)
        {
            _productRepository = productRepository;
            _billingRepository = billingRepository;
            _forexEngineRepository = forexEngineRepository;
            _shippingRepository = shippingRepository;
        }

        
        /// <summary>
        /// GET: api/<ProductController>, retrieve all products
        /// </summary>
        /// <returns></returns>
        [HttpGet("{ccyCode}")]
        public async Task<IEnumerable<ShoppingProduct>> Get(string ccyCode = "AUD")
        {
            var products = await _productRepository.GetAll(ccyCode);
            return products;
        }


        /// <summary>
        /// GET api/<ProductController>/L1HnndxVc2-KaJ10Skc/AUD
        /// </summary>
        /// <param name="key">product key, e.g.,L1HnndxVc2-KaJ10Skc </param>
        /// <param name="ccyCode">product key, e.g.,AUD </param>
        /// <returns></returns>
        [HttpGet("{key}/{ccyCode}")]
        public async Task<ShoppingProduct> Get(string key, string ccyCode = "AUD")
        {
            var product = await _productRepository.Get(key, ccyCode);
            return product;
        }
    }
}
