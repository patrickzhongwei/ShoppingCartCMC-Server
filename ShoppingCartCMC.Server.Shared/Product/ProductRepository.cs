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
using ShoppingCartCMC.Server.Shared.DB.Trading;
//using ShoppingCartCMC.Shared; //PW: do it, otherwise compile confused

namespace ShoppingCartCMC.Server.Shared.Product
{
    public class ProductRepository : iProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly iForexEngineRepository _forexEngineRepository;
        private readonly ShoppingCartCmcTradingContext _tradingContext;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="forexEngineRepository"></param>
        public ProductRepository(ILogger<ProductRepository> logger, iForexEngineRepository forexEngineRepository, ShoppingCartCmcTradingContext tradingContext)
        {
            _logger = logger;
            _forexEngineRepository = forexEngineRepository;
            _tradingContext = tradingContext;
        }


        /// <summary>
        /// get all products Dtos by ccyCode, product prices are calcuated based on market rates
        /// </summary>
        /// <param name="ccyCode">product currency code</param>
        /// <returns>a list of ProductDto</returns>
        public async Task<IEnumerable<iProductDto>> GetAll(string ccyCode)
        {
            try
            {
                List<iProduct> productsInCcy = new List<iProduct>();
                var productsDb = _tradingContext.Products.Where(p => true).ToList(); //PW: db only contain AUD product

                productsDb.ForEach(async p =>
                {
                    if (p.Currency != ccyCode)
                    {
                        try
                        {
                            decimal indirectRate = await this._forexEngineRepository.GetIndirectRate(ShippingRule.CartBaseCcy + ccyCode);
                            decimal convertedPrice = Math.Round((decimal)(p.Price * indirectRate), 0); 

                            productsInCcy.Add(new ShoppingCartCMC.Shared.Product
                            (   p.Key,
                                (int)p.Id,
                                p.Name,
                                p.Category,
                                convertedPrice, //(decimal)p.Price, //PW: modify price property
                                p.Description,
                                p.ImageUrl,
                                (long)p.TimeTickAdded,
                                (int)p.Quantity,
                                (decimal)p.Rating,
                                (bool)p.Favourite,
                                p.Seller,
                                ccyCode //p.Currency //PW: modify currency property
                            ));                            
                        }
                        catch (Exception ex)
                        {
                            //PW: if no product found by ccyCode, return products by base ccy (AUD)
                            System.Diagnostics.Debug.WriteLine(ex);
                        }
                    }
                    else
                    {
                        productsInCcy.Add(new ShoppingCartCMC.Shared.Product
                            (p.Key,
                                (int)p.Id,
                                p.Name,
                                p.Category,
                                (decimal)p.Price, 
                                p.Description,
                                p.ImageUrl,
                                (long)p.TimeTickAdded,
                                (int)p.Quantity,
                                (decimal)p.Rating,
                                (bool)p.Favourite,
                                p.Seller,
                                p.Currency 
                            ));
                    }
                });

                //PW: get from mock data source
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //List<iProduct> productsInAud = new List<iProduct>(MockData.MockProductsInBaseCcy); //PW: must clone data, otherwise it will change MockData at next call.
                             
                //productsInAud.ForEach(async eachInAud =>
                //{
                //    if (eachInAud.Currency != ccyCode)
                //    {
                //        try
                //        {
                //            decimal indirectRate = await this._forexEngineRepository.GetIndirectRate(ShippingRule.CartBaseCcy + ccyCode);
                //            eachInAud.ProductPrice = Math.Round(eachInAud.ProductPrice * indirectRate, 0); //PW: modify price property
                //            eachInAud.Currency = ccyCode; //PW: modify currency property
                //        }
                //        catch (Exception ex)
                //        {
                //            //PW: if no product found by ccyCode, return products by base ccy (AUD)
                //            System.Diagnostics.Debug.WriteLine(ex);
                //        }                        
                //    }
                //});
                //----------------------------------------------------------------------------------------------------------------------------------------------------------

                var factory = new ProductFactory();
                return factory.CreateDtoBatch(productsInCcy.ToArray()); //PW: may or maynot in AUD, depends on request ccyCode.
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// Get a product Dto by key and ccyCode
        /// </summary>
        /// <param name="key">product key</param>
        /// <param name="ccyCode">product currency code</param>
        /// <returns>a product Dto matched</returns>
        public async Task<iProductDto> Get(string key, string ccyCode)
        {
            try
            {
                var productDb = _tradingContext.Products.First(p => p.Key == key); //PW: db only contain AUD product
                ShoppingCartCMC.Shared.Product product;

                if (productDb != null)
                {
                    if (productDb.Currency != ccyCode)
                    {
                        decimal indirectRate = await this._forexEngineRepository.GetIndirectRate(ShippingRule.CartBaseCcy + ccyCode);
                        var convertedPrice = Math.Round((decimal)productDb.Price * indirectRate, 0);

                        product = new ShoppingCartCMC.Shared.Product
                                    (productDb.Key,
                                    (int)productDb.Id,
                                    productDb.Name,
                                    productDb.Category,
                                    convertedPrice, //(decimal)productDb.Price, //PW: modify price property
                                    productDb.Description,
                                    productDb.ImageUrl,
                                    (long)productDb.TimeTickAdded,
                                    (int)productDb.Quantity,
                                    (decimal)productDb.Rating,
                                    (bool)productDb.Favourite,
                                    productDb.Seller,
                                    ccyCode //productDb.Currency //PW: modify currency property
                                );
                    }
                    else
                    {
                        product = new ShoppingCartCMC.Shared.Product
                                    (productDb.Key,
                                    (int)productDb.Id,
                                    productDb.Name,
                                    productDb.Category,
                                    (decimal)productDb.Price,
                                    productDb.Description,
                                    productDb.ImageUrl,
                                    (long)productDb.TimeTickAdded,
                                    (int)productDb.Quantity,
                                    (decimal)productDb.Rating,
                                    (bool)productDb.Favourite,
                                    productDb.Seller,
                                    productDb.Currency
                                );
                    }

                    var factory = new ProductFactory();
                    return factory.CreateDto(product); //PW: may or maynot in AUD, depends on request ccyCode.
                }


                //PW: get from mock data source
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //List<iProduct> products = new List<iProduct>(MockData.MockProductsInBaseCcy); //PW: must clone data, otherwise it will change MockData at next call.
                //var foundInAud = products.Find(p => p.Productkey == key);

                //if (foundInAud != null)
                //{
                //    if (foundInAud.Currency != ccyCode)
                //    {
                //        decimal indirectRate = await this._forexEngineRepository.GetIndirectRate(ShippingRule.CartBaseCcy + ccyCode);
                //        foundInAud.ProductPrice = Math.Round(foundInAud.ProductPrice * indirectRate, 0); //PW: modify price property
                //        foundInAud.Currency = ccyCode; //PW: modify currency property
                //    }
                //}

                //var factory = new ProductFactory();
                //return factory.CreateDto(product); 
                //PW: may or maynot in AUD, depends on request ccyCode.                                                   
                //------------------------------------------------------------------------------------------------------------------------------------------------
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());                
            }

            return null;
        }

        
    }
}
