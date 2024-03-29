﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ShoppingCartCMC.Server.Shared.Product;
using ShoppingCartCMC.Server.Shared.Billing;
using ShoppingCartCMC.Server.Shared.MarketData;
using ShoppingCartCMC.Server.Shared.Shipping;
using ShoppingCartCMC.Shared.DTO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCartCMC.WebApi.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]  //PW: further development
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly iProductRepository _productRepository;
        private readonly iBillingRepository _billingRepository;
        private readonly iForexEngineRepository _forexEngineRepository;
        private readonly iShippingRepository _shippingRepository;

        public BillingController(iProductRepository productRepository, iBillingRepository billingRepository, iForexEngineRepository forexEngineRepository, iShippingRepository shippingRepository)
        {
            _productRepository = productRepository;
            _billingRepository = billingRepository;
            _forexEngineRepository = forexEngineRepository;
            _shippingRepository = shippingRepository;
        }



        /// <summary>
        /// PUT api/<BillingController>
        /// </summary>
        /// <param name="billingDto"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<int> Put(int id, [FromBody] BillingDto billingDto) //PW: buggy, 400 error, related to deseriablzation, need to create interface.
        {
            return await _billingRepository.PlaceOrder(billingDto);
        }
    }
}
