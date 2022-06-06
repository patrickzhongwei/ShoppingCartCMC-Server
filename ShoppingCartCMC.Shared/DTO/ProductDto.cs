using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared.DTO
{
    /// <summary>
    /// PW: Note!!
    /// 1- minified objects to reduce data size.
    /// 2- minification rule must be identical to client side ProductDto.
    /// </summary>
    public class ProductDto
    {
        [JsonProperty(PropertyName = "p")]
        public string Productkey { get; set; } //PW: a random string like 'LXsQ2ImqAdLlHJFAwor'

        [JsonProperty(PropertyName = "r")]
        public int ProductId { get; set; } //PW: product Id, like '10005'

        [JsonProperty(PropertyName = "o")]
        public string ProductName { get; set; } //PW: product name, like 'Nokia 8.1'

        [JsonProperty(PropertyName = "d")]
        public string ProductCategory { get; set; } //PW: product category, like 'Smartphone'

        [JsonProperty(PropertyName = "u")]
        public decimal ProductPrice { get; set; } //PW: product price amount

        [JsonProperty(PropertyName = "c")]
        public string ProductDescription { get; set; } //PW: production description

        [JsonProperty(PropertyName = "t")]
        public string ProductImageUrl { get; set; } //PW: product image url link, like 'https://i.ibb.co/g9Vk9jc/nokia8-1.jpg'

        [JsonProperty(PropertyName = "a")]
        public Int64 ProductAdded { get; set; } //PW: datetime that product is added, this is the number of seconds since the Unix Epoch

        [JsonProperty(PropertyName = "q")]
        public int ProductQuatity { get; set; } //PW: product stock number

        [JsonProperty(PropertyName = "i")]
        public decimal Ratings { get; set; } //PW: product customer ratings

        [JsonProperty(PropertyName = "f")]
        public bool Favourite { get; set; } //PW: is or isn't favourite product

        [JsonProperty(PropertyName = "s")]
        public string ProductSeller { get; set; } //PW: product seller

        [JsonProperty(PropertyName = "e")]
        public string Currency { get; set; } //PW: currency of product price
    }
}
