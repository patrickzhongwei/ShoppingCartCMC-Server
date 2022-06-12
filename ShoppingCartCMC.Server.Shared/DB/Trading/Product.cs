using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Trading
{
    public partial class Product
    {
        public string Key { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int? TimeTickAdded { get; set; }
        public int? Quantity { get; set; }
        public decimal? Rating { get; set; }
        public bool? Favourite { get; set; }
        public string Seller { get; set; }
        public string Currency { get; set; }
    }
}
