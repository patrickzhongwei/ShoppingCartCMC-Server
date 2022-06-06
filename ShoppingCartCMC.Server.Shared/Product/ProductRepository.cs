using Microsoft.Extensions.Logging;
using ShoppingCartCMC.Server.Shared.BizRule;
using ShoppingCartCMC.Server.Shared.MarketData;
using ShoppingCartCMC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingProduct = ShoppingCartCMC.Shared.Product; //PW: do it, otherwise compile confused

namespace ShoppingCartCMC.Server.Shared.Product
{
    public class ProductRepository : iProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly iForexEngineRepository _forexEngineRepository;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="forexEngineRepository"></param>
        public ProductRepository(ILogger<ProductRepository> logger, iForexEngineRepository forexEngineRepository)
        {
            _logger = logger;
            _forexEngineRepository = forexEngineRepository;
        }


        /// <summary>
        /// get all products with specific currency, product prices are calcuated based on market rates
        /// </summary>
        /// <param name="ccyCode">product currency code</param>
        /// <returns>a list of Product</returns>
        public IEnumerable<ShoppingProduct> GetAll(string ccyCode)
        {
            //PW: get from mock data source
            List<ShoppingProduct> productsInAud = new List<ShoppingProduct>(MockData.MockProductsInBaseCcy); //PW: must clone data, otherwise it will change MockData at next call.
            productsInAud.ForEach(eachInAud =>
            {
                if (eachInAud.Currency != ccyCode)
                {
                    decimal directRate = this._forexEngineRepository.GetDirectRate(ShippingRule.CartBaseCcy + ccyCode);
                    eachInAud.ProductPrice = Math.Round(eachInAud.ProductPrice / directRate, 0); 
                }
            });

            return productsInAud;
        }


        /// <summary>
        /// Get a product by key
        /// </summary>
        /// <param name="key">product key</param>
        /// <param name="ccyCode">product currency code</param>
        /// <returns>a product with key matched</returns>
        public ShoppingProduct Get(string key, string ccyCode)
        {
            //PW: get from mock data source
            List<ShoppingProduct> products = new List<ShoppingProduct>(MockData.MockProductsInBaseCcy); //PW: must clone data, otherwise it will change MockData at next call.
            var foundInAud = products.Find(p => p.Productkey == key);

            if (foundInAud != null)
            {
                if (foundInAud.Currency != ccyCode)
                {
                    decimal directRate = this._forexEngineRepository.GetDirectRate(ShippingRule.CartBaseCcy + ccyCode);
                    foundInAud.ProductPrice = Math.Round(foundInAud.ProductPrice / directRate, 0);
                }
            }
                
            return foundInAud;
        }

        
    }
}
