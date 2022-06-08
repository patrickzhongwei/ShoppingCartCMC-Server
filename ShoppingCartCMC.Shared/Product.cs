using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared
{
    /// <summary>
    /// PW: Product enity
    /// </summary>
    public class Product : iProduct
    {
        public string  Productkey { get; set; } //PW: a random string like 'LXsQ2ImqAdLlHJFAwor'
        public int     ProductId { get; set; } //PW: product Id, like '10005'
        public string  ProductName { get; set; } //PW: product name, like 'Nokia 8.1'
        public string  ProductCategory { get; set; } //PW: product category, like 'Smartphone'
        public decimal ProductPrice { get; set; } //PW: product price amount

        public string  ProductDescription { get; set; } //PW: production description
        public string  ProductImageUrl { get; set; } //PW: product image url link, like 'https://i.ibb.co/g9Vk9jc/nokia8-1.jpg'
        public Int64   ProductAdded { get; set; } //PW: datetime that product is added, this is the number of seconds since the Unix Epoch
        public int     ProductQuatity { get; set; } //PW: product stock number
        public decimal Ratings { get; set; } //PW: product customer ratings

        public bool    Favourite { get; set; } //PW: is or isn't favourite product
        public string  ProductSeller { get; set; } //PW: product seller
        public string  Currency { get; set; } //PW: currency of product price

        public Product(
            string productkey,
            int productId,
            string productName,
            string productCategory,
            decimal productPrice,

            string productDescription,
            string productImageUrl,
            Int64 productAdded,
            int productQuatity,
            decimal ratings,

            bool favourite,
            string productSeller,
            string currency )
        {
            this.Productkey      = productkey;
            this.ProductId       = productId;
            this.ProductName     = productName;
            this.ProductCategory = productCategory;
            this.ProductPrice    = productPrice;

            this.ProductDescription = productDescription;
            this.ProductImageUrl    = productImageUrl;
            this.ProductAdded       = productAdded;
            this.ProductQuatity     = productQuatity;
            this.Ratings            = ratings;

            this.Favourite      = favourite;
            this.ProductSeller  = productSeller;
            this.Currency       = currency;
        }
    }
}
