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
    /// 2- minification rule must be identical to client side BillingDto.
    /// </summary>
    public class BillingDto : iBillingDto
    {
        [JsonProperty(PropertyName = "s")]
        public decimal SubTotal { get; set; } //PW: only sum of product price

        [JsonProperty(PropertyName = "h")]
        public decimal ShippingFee { get; set; } //PW: shipping fee

        [JsonProperty(PropertyName = "t")]
        public decimal Total { get; set; } //PW: sum of product price + shipping fee

        [JsonProperty(PropertyName = "f")]
        public string? FirstName { get; set; } //PW: customer's first name
 

        [JsonProperty(PropertyName = "l")]
        public string? LastName { get; set; } //PW: customer's last name

        [JsonProperty(PropertyName = "m")]
        public string? EmailId { get; set; } //PW: customer's email id

        [JsonProperty(PropertyName = "a")]
        public string? Address1 { get; set; } //PW: customer's address1

        [JsonProperty(PropertyName = "d")]
        public string? Address2 { get; set; }//PW: customer's address2

        [JsonProperty(PropertyName = "c")]
        public string? Country { get; set; } //PW: customer's country

        [JsonProperty(PropertyName = "e")]
        public string? State { get; set; } //PW: customer's state if any

        [JsonProperty(PropertyName = "z")]
        public string? Zip { get; set; } //PW: customer's post code

        [JsonProperty(PropertyName = "p")]
        public ProductDto[]? ProductDtos { get; set; }  //PW: all products purchased by customer. Multiple same Products are added here separately, as Product 'quantity' isn't used, it will change later.

    }
}
