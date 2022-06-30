using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ShoppingCartCMC.Server.Shared.Product;
using ShoppingCartCMC.Server.Shared.Billing;
using ShoppingCartCMC.Server.Shared.MarketData;
using ShoppingCartCMC.Server.Shared.Shipping;
using ShoppingProduct = ShoppingCartCMC.Shared.Product;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        /// GET api/<ProductController>/
        /// </summary>
        /// <param name="key">product key, e.g.,L1HnndxVc2-KaJ10Skc </param>
        /// <param name="ccyCode">ccy code, e.g.,AUD </param>
        /// <returns>Product object JsonSerialized string</returns>
        [HttpGet()]
        public async Task<string> Get(string key, string ccyCode = "AUD")
        {
            /*
             * Patrick: this is for Newtonsoft to serialize             * 
             */
            //var product = await _productRepository.Get(key, ccyCode);

            //string jsonString = JsonConvert.SerializeObject(
            //product,
            //Formatting.None,
            //new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //});

            //return jsonString;


            /*
             * Patrick: this is for System.Text.Json to serialize             * 
             */
            var product = await _productRepository.Get(key, ccyCode);

            string jsonString = System.Text.Json.JsonSerializer.Serialize((object)product); //PW: must cast from interface to object[] here, otherwise only interface's property names are picked in serialization.
            return jsonString;
        }
    }
}
