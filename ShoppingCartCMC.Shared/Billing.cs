using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared
{
    /// <summary>
    /// PW: Billing entity
    /// </summary>
    public class Billing
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
        public Product[] Products { get; set; } //PW: all products purchased by customer. Multiple same Products are added here separately, as Product 'quantity' isn't used, it will change later.

        public Billing(decimal subTotal, decimal shippingFee, decimal total, string firstName, string lastName, string emailId, string address1, string address2, string country, string state, string zip, Product[] products)
        {
            this.SubTotal = subTotal;
            this.ShippingFee = shippingFee;
            this.Total = total;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailId = emailId;
            this.Address1 = address1;
            this.Address2 = address2;
            this.Country = country;
            this.State = state;
            this.Zip = zip;
            this.Products = products;
        }
    }
}
