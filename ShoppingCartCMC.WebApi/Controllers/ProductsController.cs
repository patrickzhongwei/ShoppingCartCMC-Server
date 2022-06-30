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
    public class ProductsController : ControllerBase
    {
        private readonly iProductRepository _productRepository;
        private readonly iBillingRepository _billingRepository;
        private readonly iForexEngineRepository _forexEngineRepository;
        private readonly iShippingRepository _shippingRepository;
        public ProductsController(iProductRepository productRepository, iBillingRepository billingRepository, iForexEngineRepository forexEngineRepository, iShippingRepository shippingRepository)
        {
            _productRepository = productRepository;
            _billingRepository = billingRepository;
            _forexEngineRepository = forexEngineRepository;
            _shippingRepository = shippingRepository;
        }


        /// <summary>
        /// GET api/<ProductsController>/
        /// </summary>
        /// <param name="ccyCode">ccy code, e.g.,AUD </param>
        /// <returns>Product[] object JsonSerialized string</returns>
        [HttpGet()]
        public async Task<string> Get(string ccyCode = "AUD")
        {
            /*
             * Patrick: this is for Newtonsoft to serialize             * 
             */
            //var products = await _productRepository.GetAll(ccyCode);

            //string jsonString = JsonConvert.SerializeObject(
            //products,
            //Formatting.None,
            //new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //});

            //return jsonString;


            /*
             * Patrick: this is for System.Text.Json to serialize             * 
             */
            var products = await _productRepository.GetAll(ccyCode);
            string jsonString = System.Text.Json.JsonSerializer.Serialize((object[])products); //PW: must cast from interface to object here, otherwise only interface's property names are picked in serialization.
            return jsonString;
        }
    }
}
