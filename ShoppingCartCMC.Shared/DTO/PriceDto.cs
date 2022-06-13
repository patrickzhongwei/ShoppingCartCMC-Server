using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ShoppingCartCMC.Shared.DTO
{
    
    public class PriceDto
    {
        [JsonProperty(PropertyName = "s")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "b")]
        public decimal Bid { get; set; }

        [JsonProperty(PropertyName = "a")]
        public decimal Ask { get; set; }

        /// <summary>
        /// only used for price generation
        /// </summary>
        [JsonIgnore]
        public decimal Mid { 
            get { return (this.Bid + this.Ask) / 2M; }
        }

    }
}
