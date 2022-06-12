using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Identity
{
    public partial class IdentityResourceClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int IdentityResourceId { get; set; }

        public virtual IdentityResource IdentityResource { get; set; }
    }
}
