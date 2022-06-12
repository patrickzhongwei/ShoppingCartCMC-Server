using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Trading
{
    public partial class Billing
    {
        public string Key { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? ShippingFee { get; set; }
        public decimal? Total { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public int? Zip { get; set; }
    }
}
