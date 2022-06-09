using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared.DTO
{
    public interface iBillingDto
    {
        decimal SubTotal { get; set; } //PW: only sum of product price

        decimal ShippingFee { get; set; } //PW: shipping fee

        decimal Total { get; set; } //PW: sum of product price + shipping fee

        string? FirstName { get; set; } //PW: customer's first name


        string? LastName { get; set; } //PW: customer's last name

        string? EmailId { get; set; } //PW: customer's email id

        string? Address1 { get; set; } //PW: customer's address1

        string? Address2 { get; set; }//PW: customer's address2

        string? Country { get; set; } //PW: customer's country

        string? State { get; set; } //PW: customer's state if any

        string? Zip { get; set; } //PW: customer's post code

        ProductDto[] ProductDtos { get; set; }
    }
}
