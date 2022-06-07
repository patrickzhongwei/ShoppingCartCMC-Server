using Microsoft.Extensions.Logging;
using ShoppingCartCMC.Server.Shared.BizRule;
using ShoppingCartCMC.Server.Shared.MarketData;
using ShoppingCartCMC.Shared;
using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartCMC.Shared.Factory;
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
        /// get all products Dtos by ccyCode, product prices are calcuated based on market rates
        /// </summary>
        /// <param name="ccyCode">product currency code</param>
        /// <returns>a list of ProductDto</returns>
        public async Task<IEnumerable<ProductDto>> GetAll(string ccyCode)
        {
            try
            {
                /** *
                * Patrick: [todo in future].
                * PW: await CPU-bound work here...
                */

                //PW: get from mock data source
                List<ShoppingProduct> productsInAud = new List<ShoppingProduct>(MockData.MockProductsInBaseCcy); //PW: must clone data, otherwise it will change MockData at next call.
                productsInAud.ForEach(async eachInAud =>
                {
                    if (eachInAud.Currency != ccyCode)
                    {
                        try
                        {
                            decimal indirectRate = await this._forexEngineRepository.GetIndirectRate(ShippingRule.CartBaseCcy + ccyCode);
                            eachInAud.ProductPrice = Math.Round(eachInAud.ProductPrice * indirectRate, 0); //PW: modify price property
                            eachInAud.Currency = ccyCode; //PW: modify currency property
                        }
                        catch (Exception ex)
                        {
                            //PW: if no product found by ccyCode, return products by base ccy (AUD)
                            System.Diagnostics.Debug.WriteLine(ex);
                        }                        
                    }
                });

                var factory = new ProductFactory();
                return factory.CreateDtoBatch(productsInAud.ToArray()); //PW: may or maynot in AUD, depends on request ccyCode.
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        /// <summary>
        /// Get a product Dto by key and ccyCode
        /// </summary>
        /// <param name="key">product key</param>
        /// <param name="ccyCode">product currency code</param>
        /// <returns>a product Dto matched</returns>
        public async Task<ProductDto> Get(string key, string ccyCode)
        {
            try
            {
                /** *
                * Patrick: [todo in future].
                * PW: await CPU-bound work here...
                */

                //PW: get from mock data source
                List<ShoppingProduct> products = new List<ShoppingProduct>(MockData.MockProductsInBaseCcy); //PW: must clone data, otherwise it will change MockData at next call.
                var foundInAud = products.Find(p => p.Productkey == key);
                              
                if (foundInAud != null)
                {
                    if (foundInAud.Currency != ccyCode)
                    {
                        decimal indirectRate = await this._forexEngineRepository.GetIndirectRate(ShippingRule.CartBaseCcy + ccyCode);
                        foundInAud.ProductPrice = Math.Round(foundInAud.ProductPrice * indirectRate, 0); //PW: modify price property
                        foundInAud.Currency = ccyCode; //PW: modify currency property
                    }
                }

                var factory = new ProductFactory();
                return factory.CreateDto(foundInAud); //PW: may or maynot in AUD, depends on request ccyCode.
            }
            catch(Exception ex)
            {

                return null;
            }
        }

        
    }
}
