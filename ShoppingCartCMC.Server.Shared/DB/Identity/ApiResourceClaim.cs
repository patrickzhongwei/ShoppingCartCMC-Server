using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Identity
{
    public partial class ApiResourceClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiResourceId { get; set; }

        public virtual ApiResource ApiResource { get; set; }
    }
}
