using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Identity
{
    public partial class ApiResourceScope
    {
        public int Id { get; set; }
        public string Scope { get; set; }
        public int ApiResourceId { get; set; }

        public virtual ApiResource ApiResource { get; set; }
    }
}
