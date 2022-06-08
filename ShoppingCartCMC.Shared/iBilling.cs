using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared
{
    public interface iBilling
    {
        public decimal SubTotal { get; set; } //PW: only sum of product price
        public decimal ShippingFee { get; set; } //PW: shipping fee
        public decimal Total { get; set; } //PW: sum of product price + shipping fee
        public string FirstName { get; set; } //PW: customer's first name
        public string LastName { get; set; } //PW: customer's last name
        public string EmailId { get; set; } //PW: customer's email id
        public string Address1 { get; set; } //PW: customer's address1
        public string Address2 { get; set; } //PW: customer's address2
        public string Country { get; set; } //PW: customer's country
        public string State { get; set; }//PW: customer's state if any
        public string Zip { get; set; } //PW: customer's post code
        public Product[] Products { get; set; } //PW
    }
}
