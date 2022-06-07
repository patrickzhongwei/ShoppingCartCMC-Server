using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Product
{
    public interface iProductRepository
    {
        /// <summary>
        /// get all products with specific currency, product prices are calcuated based on market rates
        /// </summary>
        /// <param name="ccyCode">product currency code</param>
        /// <returns>a list of Product</returns>
        Task<IEnumerable<ProductDto>> GetAll(string ccyCode);


        /// <summary>
        /// Get a product by key
        /// </summary>
        /// <param name="key">product key</param>
        /// <param name="ccyCode">product currency code</param>
        /// <returns>a product with key matched</returns>
        Task<ProductDto> Get(string key, string ccyCode);
    }
}
