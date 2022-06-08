using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared.DTO
{
    public interface iProductDto
    {
        string Productkey { get; set; } //PW: a random string like 'LXsQ2ImqAdLlHJFAwor'

        int ProductId { get; set; } //PW: product Id, like '10005'

        string ProductName { get; set; } //PW: product name, like 'Nokia 8.1'

        string ProductCategory { get; set; } //PW: product category, like 'Smartphone'

        decimal ProductPrice { get; set; } //PW: product price amount

        string ProductDescription { get; set; } //PW: production description

        string ProductImageUrl { get; set; } //PW: product image url link, like 'https://i.ibb.co/g9Vk9jc/nokia8-1.jpg'

        Int64 ProductAdded { get; set; } //PW: datetime that product is added, this is the number of seconds since the Unix Epoch

        int ProductQuatity { get; set; } //PW: product stock number

         decimal Ratings { get; set; } //PW: product customer ratings

        bool Favourite { get; set; } //PW: is or isn't favourite product

        string ProductSeller { get; set; } //PW: product seller

        string Currency { get; set; } //PW: currency of product price
    }
}
